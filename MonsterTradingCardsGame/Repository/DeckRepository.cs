using System.Net;
using System.Text.Json;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.MTCGServer;
using Npgsql;

namespace MonsterTradingCardsGame.Repository;

public class DeckRepository {
    private readonly NpgsqlConnection _npg;
    private static readonly object LockGet = new object();
    private static readonly object LockUpdate = new object();
    
    public DeckRepository(NpgsqlConnection npg) {
        _npg = npg;
    }

    private void CheckIfCardIsInStack(List<CardIdDTO> cards, string username) {
        foreach (var c in cards) {
            using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM stack WHERE cardid = @cardid AND fk_u_id = (SELECT u_id FROM users WHERE username = @username)", _npg);
            cmd.Parameters.AddWithValue("cardid", c.Id);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();
            if (cmd.ExecuteScalar() is not long result || result == 0)
                throw new ProcessException(HttpStatusCode.Forbidden, "At least one of the provided cards does not belong to the user or is not available\n");
        }
    }
    
    public Deck GetCardsFromDeck(string username) {
        Deck deck = new Deck();

        lock (LockGet) {
            using var command1 = new NpgsqlCommand("SELECT COUNT(*) FROM deck WHERE fk_u_id = (SELECT u_id FROM users WHERE username = @username)", _npg);
            command1.Parameters.AddWithValue("username", username);
            command1.Prepare();
            if (command1.ExecuteScalar() is not long result || result == 0)
                throw new ProcessException(HttpStatusCode.Accepted, "User has no cards in deck\n");
        
            using var command = new NpgsqlCommand("SELECT cardid, damage, name FROM deck JOIN public.cards c on deck.cardid = c.id WHERE fk_u_id = (SELECT u_id FROM users WHERE username = @username)", _npg);
            command.Parameters.AddWithValue("username", username);
            command.Prepare();
            using var reader = command.ExecuteReader();
            while (reader.Read()) {
                deck.Cards.Add(new Card(new CardDTO() {
                    Id = reader.GetString(reader.GetOrdinal("cardid")),
                    Damage = reader.GetFloat(reader.GetOrdinal("damage")),
                    Name = reader.GetString(reader.GetOrdinal("name"))
                }));
            }
            return deck;
        }
    }

    public void UpdateDeck(HTTPRequest rq, string username) {
        
        if (rq.Content is null) {
            throw new ProcessException(HttpStatusCode.BadRequest, "No content\n");
        }

        List<CardIdDTO> cardIds = new List<CardIdDTO>();
        var ids = JsonSerializer.Deserialize<string[]>(rq.Content) ?? throw new ProcessException(HttpStatusCode.InternalServerError, "");
        
        foreach (var id in ids) {
            cardIds.Add(new CardIdDTO {
                Id = id
            });
        }
        
        if(cardIds == null)              
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
        
        if (cardIds.Count != 4) {
            throw new ProcessException(HttpStatusCode.BadRequest, "Deck must contain 4 cards\n");
        }

        lock (LockUpdate) {
            CheckIfCardIsInStack(cardIds, username);

            foreach (var cardId in cardIds) {
                using var command = new NpgsqlCommand("INSERT INTO deck (fk_u_id, cardid) VALUES ((SELECT u_id FROM users WHERE username = @username), @cardid)", _npg);
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("cardid", cardId.Id);
                command.Prepare();
                var result = command.ExecuteNonQuery();
                if (result < 1)
                    throw new ProcessException(HttpStatusCode.InternalServerError, "");
            }
        }
        
        
    }

    public void DeleteDeck(string loser) {
        using var command = new NpgsqlCommand("DELETE FROM deck WHERE fk_u_id = (SELECT u_id FROM users WHERE username = @username)", _npg);
        command.Parameters.AddWithValue("username", loser);
        command.Prepare();
        var result = command.ExecuteNonQuery();
        if (result < 1)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
    }
}