using System.Net;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Logic;
using Npgsql;

namespace MonsterTradingCardsGame.Repository;

public class BattleRepository {
    
    private readonly NpgsqlConnection _npg;
    
    public BattleRepository(NpgsqlConnection npg) {
        _npg = npg;
    }
    
    public void CreateBattle(ResultDTO result) {
        using var cmd = new NpgsqlCommand("INSERT INTO battle (playerid, enemyid, status1, battlelog, status2) VALUES ((SELECT u_id FROM users WHERE username = @username1), (SELECT u_id FROM users WHERE username = @username2), @status1, @battlelog, @status2)", _npg);
        cmd.Parameters.AddWithValue("username1", result.Player1.Username);
        cmd.Parameters.AddWithValue("username2", result.Player2.Username);
        cmd.Parameters.AddWithValue("status1", result.Player1.Won);
        cmd.Parameters.AddWithValue("battlelog", result.BattleLog);
        cmd.Parameters.AddWithValue("status2", result.Player2.Won);
        var rows = cmd.ExecuteNonQuery();
        if (rows < 1) {
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
        }
    }
    
    public List<BattleDTO> GetBattleHistory(string username) {
        using var cmd = new NpgsqlCommand("SELECT u.username AS enemy, u2.username AS player, status1, status2, battlelog FROM battle " +
                                          "JOIN public.users u on u.u_id = battle.enemyid " +
                                          "JOIN public.users u2 on u2.u_id = battle.playerid " +
                                          "WHERE playerid = (SELECT u_id FROM users WHERE username = @username) " +
                                          "OR enemyid = (SELECT u_id FROM users WHERE username = @username)", _npg);
        cmd.Parameters.AddWithValue("username", username);
        using var reader = cmd.ExecuteReader();
        var history = new List<BattleDTO>();
        while (reader.Read()) {
            history.Add(new BattleDTO {
                Username1 = reader.GetString(reader.GetOrdinal("player")),
                Username2 = reader.GetString(reader.GetOrdinal("enemy")),
                Status1 = reader.GetBoolean(reader.GetOrdinal("status1")),
                Status2 = reader.GetBoolean(reader.GetOrdinal("status2")),
                BattleLog = reader.GetString(reader.GetOrdinal("battlelog"))
            });
        }
        return history;
    }
}