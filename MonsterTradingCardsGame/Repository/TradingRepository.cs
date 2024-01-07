using System.Net;
using System.Text.Json;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;
using Npgsql;

namespace MonsterTradingCardsGame.Repository;

public class TradingRepository {
    
    private readonly NpgsqlConnection _npg;
    private static readonly object LockDel = new();
    private static readonly object LockCreate = new();
    private static readonly object LockTrade = new();
    
    public TradingRepository(NpgsqlConnection npg) {
        _npg = npg;
    }
    
    public List<TradingDTO> GetTradingDeals(string username) {
        
        List<TradingDTO> tradingDeals = new List<TradingDTO>();
        using var cmd = new NpgsqlCommand("SELECT * FROM tradingoffer WHERE traded = False AND fk_u_id != (SELECT u_id FROM users WHERE username = @username)", _npg);
        cmd.Parameters.AddWithValue("username", username);
        cmd.Prepare();
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            tradingDeals.Add(new TradingDTO {
                Id = reader.GetString(reader.GetOrdinal("tradeid")),
                CardToTrade = reader.GetString(reader.GetOrdinal("cardid")),
                Type = reader.GetString(reader.GetOrdinal("type")),
                MinimumDamage = reader.GetFloat(reader.GetOrdinal("mindamage"))
            });
        }
        return tradingDeals;
    }

    public void DeleteTradingDeal(string id, string username) {
        lock (LockDel) {
            CheckIfTradingDealExistsForDelete(id);
            CheckIfCardIsInStack(GetCardIdFromTradeForDel(id), username);
            
            using var cmd = new NpgsqlCommand("DELETE FROM tradingoffer WHERE tradeid = @id AND traded = False", _npg);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Prepare();
            var result = cmd.ExecuteNonQuery();
            if (result < 1)
                throw new ProcessException(HttpStatusCode.InternalServerError, "Could not delete trading deal\n");
        }
    }

    private string GetCardIdFromTradeForDel(string id) {
        using var cmd = new NpgsqlCommand("SELECT cardid FROM tradingoffer WHERE tradeid = @id", _npg);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Prepare();
        using var reader = cmd.ExecuteReader();
        reader.Read();
        return reader.GetString(reader.GetOrdinal("cardid"));
    }

    private void CheckIfTradingDealExistsForDelete(string tradeid) {
        using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM tradingoffer WHERE tradeid = @id", _npg);
        cmd.Parameters.AddWithValue("id", tradeid);
        cmd.Prepare();
        if (cmd.ExecuteScalar() is not long result || result == 0)
            throw new ProcessException(HttpStatusCode.NotFound, "The provided deal ID was not found\n");
    }

    private void CheckIfTradingDealExistsForCreate(string tradeid) {
        using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM tradingoffer WHERE tradeid = @id", _npg);
        cmd.Parameters.AddWithValue("id", tradeid);
        cmd.Prepare();
        if (cmd.ExecuteScalar() is not long result || result == 1)
            throw new ProcessException(HttpStatusCode.Conflict, "A deal with this deal ID already exists\n");
    }
    
    private void CheckIfCardIsInStack(string cardid, string username) {
        using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM stack WHERE cardid = @id AND fk_u_id = (SELECT u_id FROM users WHERE username = @username)", _npg);
        cmd.Parameters.AddWithValue("id", cardid);
        cmd.Parameters.AddWithValue("username", username);
        cmd.Prepare();
        if (cmd.ExecuteScalar() is not long result || result == 0)
            throw new ProcessException(HttpStatusCode.Forbidden, "The deal contains a card that is not owned by the user\n");
    }
    
    private void CheckIfCardIsInDeck(string id, string username) {
        using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM deck WHERE cardid = @id AND fk_u_id = (SELECT u_id FROM users WHERE username = @username)", _npg);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("username", username);
        cmd.Prepare();
        if (cmd.ExecuteScalar() is not long result || result == 1)
            throw new ProcessException(HttpStatusCode.Forbidden, "Card is locked in a deck\n");
    }
    
    private void CheckIfUserTradsWithHimself(string id, string username) {
        using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM tradingoffer WHERE tradeid = @id AND fk_u_id = (SELECT u_id FROM users WHERE username = @username)", _npg);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("username", username);
        cmd.Prepare();
        if (cmd.ExecuteScalar() is not long result || result == 1)
            throw new ProcessException(HttpStatusCode.Forbidden, "User is trying to trade with himself\n");
    }

    private CardTradeDTO GetCardIdFromTrade(string tradeid) {
        using var cmd = new NpgsqlCommand("SELECT cardid, fk_u_id, damage, name, cardtype FROM tradingoffer JOIN public.cards c on c.id = tradingoffer.cardid WHERE tradeid = @id", _npg);
        cmd.Parameters.AddWithValue("id", tradeid);
        cmd.Prepare();
        using var reader = cmd.ExecuteReader();
        reader.Read();
        CardTradeDTO card = new CardTradeDTO() {
            Damage = reader.GetFloat(reader.GetOrdinal("damage")),
            Type = reader.GetString(reader.GetOrdinal("cardtype")),
            UserId = reader.GetInt32(reader.GetOrdinal("fk_u_id"))
        };
        return card;
    }

    private bool CheckIfRequirementsAreMet(TradingDTO trading, CardTradeDTO card) {
        if (!string.Equals(trading.Type, card.Type, StringComparison.OrdinalIgnoreCase))
            return false;
        if (trading.MinimumDamage > card.Damage)
            return false;
        return true;
    }

    public void CreateTradingDeal(HTTPRequest rq, string username) {
        if (rq.Content == null)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
        
        var trading = JsonSerializer.Deserialize<TradingDTO>(rq.Content);
        
        if (trading == null)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
        lock (LockCreate) {
            CheckIfCardIsInStack(trading.CardToTrade, username);
            CheckIfCardIsInDeck(trading.CardToTrade, username);
            CheckIfTradingDealExistsForCreate(trading.Id);
            using var cmd = new NpgsqlCommand("INSERT INTO tradingoffer (tradeid, cardid, type, mindamage, fk_u_id) VALUES (@tradeid, @cardid, @type, @mindamage, (SELECT u_id FROM users WHERE username = @username))", _npg);
            cmd.Parameters.AddWithValue("tradeid", trading.Id);
            cmd.Parameters.AddWithValue("cardid", trading.CardToTrade);
            cmd.Parameters.AddWithValue("type", trading.Type);
            cmd.Parameters.AddWithValue("mindamage", trading.MinimumDamage);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();
            var result = cmd.ExecuteNonQuery();
            if (result < 1)
                throw new ProcessException(HttpStatusCode.InternalServerError, "");
        }
        
    }

    private TradingDTO GetTradingDeal(string tradeid, string username) {
        using var cmd = new NpgsqlCommand("SELECT * FROM tradingoffer WHERE tradeid = @id AND traded = False AND fk_u_id != (SELECT u_id FROM users WHERE username = @username)", _npg);
        cmd.Parameters.AddWithValue("id", tradeid);
        cmd.Parameters.AddWithValue("username", username);
        cmd.Prepare();
        using var reader = cmd.ExecuteReader();
        reader.Read();
        TradingDTO trading = new TradingDTO() {
            Id = reader.GetString(reader.GetOrdinal("tradeid")),
            CardToTrade = reader.GetString(reader.GetOrdinal("cardid")),
            Type = reader.GetString(reader.GetOrdinal("type")),
            MinimumDamage = reader.GetFloat(reader.GetOrdinal("mindamage"))
        };
        return trading;
    }

    public void TradeCards(HTTPRequest rq, string username) {
        if (rq.Path is null)
            throw new ProcessException(HttpStatusCode.NotFound);
        
        if(rq.Content is null)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");

        string tradeid = rq.Path[1];
        
        if (string.IsNullOrEmpty(tradeid))
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
        
        string cardIdTrade = JsonSerializer.Deserialize<string>(rq.Content) ?? string.Empty;
        
        if (string.IsNullOrEmpty(cardIdTrade))
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
        
        lock (LockTrade) {
            CheckIfTradingDealExistsForDelete(tradeid);
            CheckIfUserTradsWithHimself(tradeid, username);
            var cardToTrade = GetCardIdFromTrade(tradeid);
            var trading = GetTradingDeal(tradeid, username);
            if (!CheckIfRequirementsAreMet(trading, cardToTrade))
                throw new ProcessException(HttpStatusCode.Forbidden, "The card does not meet the requirements of the trading deal\n");
            
            using var cmd = new NpgsqlCommand("UPDATE stack SET fk_u_id = @userid WHERE cardid = @cardid; UPDATE stack SET fk_u_id = (SELECT u_id FROM users WHERE username = @usernametrader) WHERE cardid = @cardidtrader", _npg);
            cmd.Parameters.AddWithValue("userid", cardToTrade.UserId);
            cmd.Parameters.AddWithValue("cardid", cardIdTrade);
            cmd.Parameters.AddWithValue("usernametrader", username);
            cmd.Parameters.AddWithValue("cardidtrader", trading.CardToTrade);
            cmd.Prepare();
            var result = cmd.ExecuteNonQuery();
            if (result < 1)
                throw new ProcessException(HttpStatusCode.InternalServerError, "");
            
            using var cmd2 = new NpgsqlCommand("UPDATE tradingoffer SET traded = True WHERE tradeid = @tradeid;", _npg);
            cmd2.Parameters.AddWithValue("tradeid", tradeid);
            cmd2.Prepare();
            var result2 = cmd2.ExecuteNonQuery();
            if (result2 < 1)
                throw new ProcessException(HttpStatusCode.InternalServerError, "");
            
            using var cmd3 = new NpgsqlCommand("INSERT INTO trading (cardid, fk_u_id, fk_to_id) VALUES (@cardid, (SELECT u_id FROM users WHERE username = @username), @tradeid)", _npg); 
            cmd3.Parameters.AddWithValue("cardid", cardIdTrade);
            cmd3.Parameters.AddWithValue("username", username);
            cmd3.Parameters.AddWithValue("tradeid", tradeid);
            cmd3.Prepare();
            var result3 = cmd3.ExecuteNonQuery();
            if (result3 < 1)
                throw new ProcessException(HttpStatusCode.InternalServerError, "");
        }
    }
    
    public List<TradingHistoryDTO> GetTradingHistory(string username) {
        List<TradingHistoryDTO> tradingDeals = new List<TradingHistoryDTO>();
        using var cmd = new NpgsqlCommand("SELECT t.tradeid, t.cardid AS o_cardid, trading.cardid AS t_cardid, mindamage, type, t.fk_u_id AS offerer_id, trading.fk_u_id AS trader_id FROM trading " +
                                          "JOIN public.tradingoffer t on t.tradeid = trading.fk_to_id " +
                                          "WHERE (t.fk_u_id = (SELECT u_id FROM users WHERE username = @username) " +
                                          "OR trading.fk_u_id = (SELECT u_id FROM users WHERE username = @username)) " +
                                          "AND traded = True", _npg);
        cmd.Parameters.AddWithValue("username", username);
        cmd.Prepare();
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            tradingDeals.Add(new TradingHistoryDTO {
                Id = reader.GetString(reader.GetOrdinal("tradeid")),
                Offerer = reader.GetInt32(reader.GetOrdinal("offerer_id")),
                CardToTrade = reader.GetString(reader.GetOrdinal("o_cardid")),
                Type = reader.GetString(reader.GetOrdinal("type")),
                MinimumDamage = reader.GetFloat(reader.GetOrdinal("mindamage")),
                Trader = reader.GetInt32(reader.GetOrdinal("trader_id")),
                CardToReceive = reader.GetString(reader.GetOrdinal("t_cardid"))
            });
        }
        return tradingDeals;
    }

    public List<TradingHistoryDetailedDTO> GetTradingHistoryDetailed(string username) {
        List<TradingHistoryDetailedDTO> tradingDeals = new List<TradingHistoryDetailedDTO>();
        using var cmd = new NpgsqlCommand(@"SELECT u.username AS o_user, t.tradeid, t.cardid AS o_cardid, mindamage, type, u2.username AS t_user,  trading.cardid AS o_cardid, c.name AS o_cname, c.damage AS o_cdamage, c.cardtype AS o_ctype, c2.name AS t_cname, c2.damage AS t_cdamage, c2.cardtype AS t_ctype FROM trading 
                                        JOIN public.tradingoffer t on t.tradeid = trading.fk_to_id 
                                        JOIN public.users u2 on u2.u_id = trading.fk_u_id 
                                        JOIN public.users u on u.u_id = t.fk_u_id 
                                        JOIN public.cards c on c.id = trading.cardid 
                                        JOIN public.cards c2 on c2.id = t.cardid 
                                             WHERE (t.fk_u_id = (SELECT u_id FROM users WHERE username = @username) 
                                                        OR trading.fk_u_id = (SELECT u_id FROM users WHERE username = @username) ) 
                                               AND traded = True", _npg);
        cmd.Parameters.AddWithValue("username", username);
        cmd.Prepare();
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            tradingDeals.Add(new TradingHistoryDetailedDTO {
                Id = reader.GetString(reader.GetOrdinal("tradeid")),
                Offerer = reader.GetString(reader.GetOrdinal("o_user")),
                CardToTrade = reader.GetString(reader.GetOrdinal("o_cardid")),
                Type = reader.GetString(reader.GetOrdinal("type")),
                MinimumDamage = reader.GetFloat(reader.GetOrdinal("mindamage")),
                Trader = reader.GetString(reader.GetOrdinal("t_user")),
                CardToReceive = reader.GetString(reader.GetOrdinal("o_cardid")),
                OffererCardName = reader.GetString(reader.GetOrdinal("o_cname")),
                OffererCardType = reader.GetString(reader.GetOrdinal("o_ctype")),
                OffererCardDamage = reader.GetFloat(reader.GetOrdinal("o_cdamage")),
                TraderCardName = reader.GetString(reader.GetOrdinal("t_cname")),
                TraderCardType = reader.GetString(reader.GetOrdinal("t_ctype")),
                TraderCardDamage = reader.GetFloat(reader.GetOrdinal("t_cdamage"))
            });
        }
        return tradingDeals;
    }
}