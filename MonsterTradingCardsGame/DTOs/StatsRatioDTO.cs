namespace MonsterTradingCardsGame.DTOs;

public class StatsRatioDTO {
    public string Name { get; set; } = "";
    public float Losses { get; set; }
    public float Wins { get; set; }
    
    public string WinRatio() {
        if (Wins == 0) {
            return "Wins: 0%";
        }
        float ratio = (Wins / (Wins + Losses)) * 100;
        return $"Wins: {ratio}%\n";
    }
}