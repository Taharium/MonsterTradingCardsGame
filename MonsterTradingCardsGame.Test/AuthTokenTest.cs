using MonsterTradingCardsGame.Extensions;

namespace MonsterTradingCardsGame.Test;

public class AuthTokenTest
{
    [Test]
    public void TestAuthToken() {
        Dictionary<string, string> header = new Dictionary<string, string>();
        string username = "testUser";
        string token = AuthToken.GenerateToken(username);
        
        string[] parts = token.Split(':');
        header.Add(parts[0], parts[1].Trim());
        string newToken = header["Authorization"];
        
        Assert.That(header["Authorization"], Is.EqualTo($"Bearer {username}-mtcgToken"));
        Assert.That(AuthToken.ParseTokenForUsername(newToken), Is.EqualTo(username));
    }
}