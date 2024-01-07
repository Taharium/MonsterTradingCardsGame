using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DTOs;

public class ResultDTO {
    public string BattleLog { get; set; } = "Entered Lobby\n";

    public Player Player1 { get; set; } = new Player();
    public Player Player2 { get; set; } = new Player();
    public bool Draw { get; set; }
}