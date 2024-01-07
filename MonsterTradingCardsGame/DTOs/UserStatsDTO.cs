namespace MonsterTradingCardsGame.DTOs;

public class UserStatsDTO {
	public string Name { get; set; } = "";
	public int Losses { get; set; }
	public int Wins { get; set; }
	public int Elo { get; set; }
}
