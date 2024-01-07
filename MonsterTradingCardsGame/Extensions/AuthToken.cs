namespace MonsterTradingCardsGame.Extensions;

public static class AuthToken {
    public static string GenerateToken(string username) {
        return $"Authorization: Bearer {username}-mtcgToken";
    }
    
    public static string ParseTokenForUsername(string token) {
        string[] tokenParts = token.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        string[] tokenParts2 = tokenParts[1].Split('-', StringSplitOptions.RemoveEmptyEntries);
        return tokenParts2[0];
    }
}