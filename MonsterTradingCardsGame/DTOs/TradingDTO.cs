
namespace MonsterTradingCardsGame.DTOs;

public class TradingDTO {
    public string Id { get; set; } = "";
    public string CardToTrade { get; set; } = "";
    public string Type { get; set; } = "";
    public float MinimumDamage { get; set; }
}