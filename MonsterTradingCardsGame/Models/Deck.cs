using System.Text;
namespace MonsterTradingCardsGame.Models;

public class Deck {
    public List<Card> Cards { get; set; } = new List<Card>();
    public string ToCustomString() {
        StringBuilder stringBuilder = new StringBuilder();
        
        stringBuilder.Append("[\n");

        for (int i = 0; i < Cards.Count; i++) {
            stringBuilder.Append("\t");
            stringBuilder.Append(Cards[i].ToCustomString());
            stringBuilder.Append("\n");
        }

        stringBuilder.Append("]");
        
        return stringBuilder.ToString();
    }
}