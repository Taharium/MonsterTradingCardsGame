using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;
using Npgsql;
using System.Net;
using System.Text.Json;
using MonsterTradingCardsGame.DTOs;

namespace MonsterTradingCardsGame.Repository;

internal class UserRepository {
        
    private readonly NpgsqlConnection _npg;
    private static readonly object LockCreate = new object();
    private static readonly object LockUpdate = new object();
        
    public UserRepository(NpgsqlConnection npg) {
        _npg = npg;
    }


    public void CreateUser(HTTPRequest rq) {
        if (rq.Content == null)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
            
        UserCredDTO? user = JsonSerializer.Deserialize<UserCredDTO>(rq.Content);
        if (user == null)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");

        if (user.Username == null || user.Password == null)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");

        lock (LockCreate) {
            using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE username = @username", _npg)) {
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Prepare();
                
                if (cmd.ExecuteScalar() is not long result || result > 0)
                    throw new ProcessException(HttpStatusCode.Conflict, "Username already exists\n");
            }

            using var command = new NpgsqlCommand("INSERT INTO users (username, password) VALUES (@username, @password) RETURNING u_id", _npg);
            command.Parameters.AddWithValue("username", user.Username);
            command.Parameters.AddWithValue("password", user.Password);

            if (command.ExecuteScalar() is not int)
                throw new ProcessException(HttpStatusCode.InternalServerError, "");
        }
        
    }

    public UserDataDTO GetUserByUsername(HTTPRequest rq) {
        if (rq.Path is null || rq.Path.Length < 2)
            throw new ProcessException(HttpStatusCode.NotFound, "User not found\n");

        UserDataDTO user = new();
        var username = rq.Path[1];
            
        using var cmd = new NpgsqlCommand("SELECT image, bio, name FROM users WHERE username = @username", _npg);
        cmd.Parameters.AddWithValue("username", username);
        using var reader = cmd.ExecuteReader();
        reader.Read();
        if (reader.HasRows) {
            user = new UserDataDTO() {
                Image = reader.IsDBNull(reader.GetOrdinal("image")) ? "" : reader.GetString(reader.GetOrdinal("image")),
                Bio = reader.IsDBNull(reader.GetOrdinal("bio")) ? "" : reader.GetString(reader.GetOrdinal("bio")),
                Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader.GetString(reader.GetOrdinal("name"))
            };
        }
        return user;
    }


    public void UpdateNameBioImage(HTTPRequest rq, string username) {
        if (rq.Content == null)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");

        UserDataDTO user = JsonSerializer.Deserialize<UserDataDTO>(rq.Content) ?? throw new ProcessException(HttpStatusCode.InternalServerError, "");

        using var cmd = new NpgsqlCommand("UPDATE users SET bio = @bio, name = @name, image = @image WHERE username = @username", _npg);
        cmd.Parameters.AddWithValue("bio", user.Bio);
        cmd.Parameters.AddWithValue("name", user.Name);
        cmd.Parameters.AddWithValue("image", user.Image);
        cmd.Parameters.AddWithValue("username", username);

        cmd.Prepare();
        int res = cmd.ExecuteNonQuery();
            
        if (res <= 0) 
            throw new ProcessException(HttpStatusCode.NotFound, "User not found\n");
    }

    public void UpdateCoinsLost(string username) {
        using var cmd = new NpgsqlCommand("UPDATE users SET coins = (coins-5) WHERE username = @username", _npg);
        cmd.Parameters.AddWithValue("username", username);
        cmd.Prepare();
        int res = cmd.ExecuteNonQuery();
            
        if (res <= 0)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
    }
    
    public void UpdateCoinsWon(string username) {
        using var cmd = new NpgsqlCommand("UPDATE users SET coins = (coins+2) WHERE username = @username", _npg);
        cmd.Parameters.AddWithValue("username", username);
        cmd.Prepare();
        int res = cmd.ExecuteNonQuery();
            
        if (res <= 0)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
    }

    public UserStatsDTO GetStats(string username) {
        UserStatsDTO user = new();
        using var cmd = new NpgsqlCommand("SELECT name, elo, wins, losses FROM users WHERE username = @username", _npg);
        cmd.Parameters.AddWithValue("username", username);
        cmd.Prepare();
        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            throw new ProcessException(HttpStatusCode.NotFound, "User not found\n");
            
        if (reader.HasRows) {
            user = new UserStatsDTO {
                Name = reader.GetString(reader.GetOrdinal("name")),
                Elo = reader.GetInt32(reader.GetOrdinal("elo")),
                Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                Losses = reader.GetInt32(reader.GetOrdinal("losses")),
            };
        }
            
        return user;
    }
        
    public List<UserStatsDTO> GetScoreboard() {
        List<UserStatsDTO> scoreboard = new();
        using var cmd = new NpgsqlCommand("SELECT name, elo, wins, losses FROM users WHERE username != 'admin' ORDER BY elo DESC", _npg);
        cmd.Prepare();
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            scoreboard.Add(new UserStatsDTO {
                Name = reader.GetString(reader.GetOrdinal("name")),
                Elo = reader.GetInt32(reader.GetOrdinal("elo")),
                Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                Losses = reader.GetInt32(reader.GetOrdinal("losses")),
            });
        }
        return scoreboard;
    }
        
    public void UpdateUserStats(string winner, string loser) {
        lock (LockUpdate) {
            using var cmd = new NpgsqlCommand("UPDATE users SET wins = (wins+1), elo = (elo+3) WHERE username = @winner; " + 
                                              "UPDATE users SET losses = (losses+1), elo = (elo-5) WHERE username = @loser ", _npg);
            cmd.Parameters.AddWithValue("winner", winner);
            cmd.Parameters.AddWithValue("loser", loser);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }

    public StatsRatioDTO GetStatsRatio(string username) {
        StatsRatioDTO stats = new();
        using var cmd = new NpgsqlCommand("SELECT name, wins, losses FROM users WHERE username = @username", _npg);
        cmd.Parameters.AddWithValue("username", username);
        cmd.Prepare();
        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            throw new ProcessException(HttpStatusCode.NotFound, "User not found\n");
            
        if (reader.HasRows) {
            stats = new StatsRatioDTO {
                Name = reader.GetString(reader.GetOrdinal("name")),
                Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                Losses = reader.GetInt32(reader.GetOrdinal("losses")),
            };
        }
        return stats;
    }
}