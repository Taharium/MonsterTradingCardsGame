using System.Text;

namespace MonsterTradingCardsGame.Extensions;

public static class ListExtensions {
    public static string ToCustomString<T>(this List<T> list) {
        StringBuilder stringBuilder = new StringBuilder();
        
        if(list.Count == 0)
            return "[]";
        
        stringBuilder.Append("[\n");
    
        for (int i = 0; i < list.Count; i++) {
            stringBuilder.Append("\t");
            stringBuilder.Append(list[i]?.ToString());
            if (i < list.Count) {
                stringBuilder.Append("\n");
            }
        }
    
        stringBuilder.Append("]");
    
        return stringBuilder.ToString();
    }
}