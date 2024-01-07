using System.Net;
using System.Net.Mime;
using System.Text.Json;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;
using Npgsql;

namespace MonsterTradingCardsGame.Repository;

public class SessionRepository {
    private readonly NpgsqlConnection _npg;
    
    public SessionRepository(NpgsqlConnection npg) {
        _npg = npg;
    }
    
    public void CreateSession(HTTPRequest rq, HTTPResponse rs) {
        if (rq.Content == null)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");

        var loginDto = JsonSerializer.Deserialize<UserCredDTO>(rq.Content);
        
        if (loginDto == null)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");

        if (loginDto.Username == null || loginDto.Password == null)
            throw new ProcessException(HttpStatusCode.Unauthorized, "Invalid username/password provided\n");

        using var command = new NpgsqlCommand("SELECT u_id, username, password FROM users WHERE username = @username AND password = @password", _npg);
        command.Parameters.AddWithValue("username", loginDto.Username);
        command.Parameters.AddWithValue("password", loginDto.Password);
        
        using var reader = command.ExecuteReader();
        if (!reader.Read())
            throw new ProcessException(HttpStatusCode.Unauthorized, "Invalid username/password provided\n");

        string password = reader.GetString(reader.GetOrdinal("password"));
        string username = reader.GetString(reader.GetOrdinal("username"));
        int id = reader.GetInt32(reader.GetOrdinal("u_id"));
        if (password != loginDto.Password || username != loginDto.Username)
            throw new ProcessException(HttpStatusCode.Unauthorized, "Invalid username/password provided\n");
        
        reader.Close();
        CheckIfAlreadyLoggedIn(id);
        string token = InsertToken(username, id);
        rs.Prepare(HttpStatusCode.OK, token, MediaTypeNames.Text.Plain);
    }

    private void CheckIfAlreadyLoggedIn(int id) {
        using var command = new NpgsqlCommand("SELECT COUNT(*) FROM auth_tokens WHERE fk_u_id = @fk_u_id", _npg);
        command.Parameters.AddWithValue("fk_u_id", id);
        if (command.ExecuteScalar() is not long result || result > 0)
            throw new ProcessException(HttpStatusCode.Conflict, "User is already logged in\n");
    }

    private string InsertToken(string username, int id) {
        string token = AuthToken.GenerateToken(username).Split(" ")[2];
        
        using var command2 = new NpgsqlCommand("INSERT INTO auth_tokens (fk_u_id, token) VALUES (@fk_u_id, @token)", _npg);
        command2.Parameters.AddWithValue("fk_u_id", id);
        command2.Parameters.AddWithValue("token", token);
        command2.Prepare();
        
        int res = command2.ExecuteNonQuery();
        if (res <= 0)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
        return token;
    }
    
    public void CheckToken(string username) {
        using var command = new NpgsqlCommand("SELECT token FROM auth_tokens WHERE fk_u_id = (SELECT u_id FROM users WHERE username = @username)", _npg);
        command.Parameters.AddWithValue("username", username);
        using var reader = command.ExecuteReader();
        if (!reader.Read())
            throw new ProcessException(HttpStatusCode.Unauthorized, "Access token is missing or invalid\n");
    }

    private bool CheckIfUserExists(string username) {
        using var command = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE username = @username", _npg);
        command.Parameters.AddWithValue("username", username);
        command.Prepare();
        using var reader = command.ExecuteReader();
        reader.Read();
        return reader.GetInt32(reader.GetOrdinal("count")) > 0;
    }
    
    public void CheckTokenInHeader(HTTPRequest rq) { 
        if (rq.Headers["Authorization"] == null)
            throw new ProcessException(HttpStatusCode.Unauthorized, "Access token is missing or invalid\n");
        
        if(rq.Path == null || rq.Path.Length < 2)
            throw new ProcessException(HttpStatusCode.Unauthorized, "Access token is missing or invalid\n");
        
        var usernameToAccess = rq.Path[1];
        var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
        if(!CheckIfUserExists(usernameToAccess))
            throw new ProcessException(HttpStatusCode.NotFound, "User not found\n");
        if(username != usernameToAccess && !username.Contains("admin"))
            throw new ProcessException(HttpStatusCode.Unauthorized, "Access token is missing or invalid\n");
        
        CheckToken(username);
    }
}