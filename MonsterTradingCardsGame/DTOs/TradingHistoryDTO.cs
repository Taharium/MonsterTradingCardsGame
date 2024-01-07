namespace MonsterTradingCardsGame.DTOs;

public class TradingHistoryDTO {
    public string Id { get; set; } = "";
    public int Offerer { get; set; }
    public string CardToTrade { get; set; } = "";
    public string Type { get; set; } = "";
    public float MinimumDamage { get; set; }
    public string CardToReceive { get; set; } = "";
    public int Trader { get; set; }
    
    public override string ToString() {
        return $"TradingId: {Id}; Offerer: {Offerer}; CardToTrade: {CardToTrade};\n\t\tType: {Type}; MinimumDamage: {MinimumDamage}; Trader: {Trader}; CardToReceive: {CardToReceive},";
    }
}