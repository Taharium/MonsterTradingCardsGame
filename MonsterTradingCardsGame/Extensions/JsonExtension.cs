using System.Net;
using System.Text.Json;
using MonsterTradingCardsGame.Logic;

namespace MonsterTradingCardsGame.Extensions;

public static class JsonExtension {
    
    public static string Beautify<T>(this T obj) {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }) ?? throw new ProcessException(HttpStatusCode.InternalServerError, "");
    }
}