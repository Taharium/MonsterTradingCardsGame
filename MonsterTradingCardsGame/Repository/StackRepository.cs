using System.Net;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.Models;
using Npgsql;

namespace MonsterTradingCardsGame.Repository;

public class StackRepository {
    
    private readonly NpgsqlConnection _npg;
    private static readonly object LockGet = new object();
    public StackRepository(NpgsqlConnection npg) {
        _npg = npg;
    }
    
    public void InsertIntoStack(string username, List<CardDTO> cards) {
        foreach (CardDTO c in cards) {
            var insertCommand = new NpgsqlCommand("INSERT INTO stack (cardid, fk_u_id) VALUES (@cardid, (SELECT u_id FROM users WHERE username = @username))", _npg);
            insertCommand.Parameters.AddWithValue("cardid", c.Id);
            insertCommand.Parameters.AddWithValue("username", username);

            insertCommand.Prepare();
            if (insertCommand.ExecuteNonQuery() != 1)
                throw new ProcessException(HttpStatusCode.InternalServerError, "");
        }
    }
    
    public Stack GetCardsFromStack(string username) {
        Stack stack = new Stack();

        lock (LockGet) {
            using var command1 = new NpgsqlCommand("SELECT COUNT(*) FROM stack WHERE fk_u_id = (SELECT u_id FROM users WHERE username = @username)", _npg);
            command1.Parameters.AddWithValue("username", username);
            command1.Prepare();
            if (command1.ExecuteScalar() is not long result || result == 0)
                throw new ProcessException(HttpStatusCode.Accepted, "User has no cards in stack\n");
        
            using var command = new NpgsqlCommand("SELECT cardid, name, damage FROM stack JOIN public.cards c on stack.cardid = c.id WHERE fk_u_id = (SELECT u_id FROM users WHERE username = @username)", _npg);
            command.Parameters.AddWithValue("username", username);
            command.Prepare();
            using var reader = command.ExecuteReader();
            while (reader.Read()) {
                stack.Cards.Add(new Card(new CardDTO() {
                        Id = reader.GetString(reader.GetOrdinal("cardid")),
                        Damage = reader.GetFloat(reader.GetOrdinal("damage")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                    })
                );
            }
            return stack;
        }
    }


    public void UpdateStack(Player winner, string loser) {
        foreach(Card c in winner.PlayerDeck.Cards) {
            var cmd = new NpgsqlCommand("UPDATE stack SET fk_u_id = (SELECT u_id FROM users WHERE username = @winner) WHERE cardid = @cardid AND fk_u_id = (SELECT u_id FROM users WHERE username = @loser)", _npg);
            cmd.Parameters.AddWithValue("cardid", c.Id);
            cmd.Parameters.AddWithValue("winner", winner.Username);
            cmd.Parameters.AddWithValue("loser", loser);
            cmd.Prepare();
            var result = cmd.ExecuteNonQuery();
            if (result == -1)
                throw new ProcessException(HttpStatusCode.InternalServerError, "");
        }
    }
}