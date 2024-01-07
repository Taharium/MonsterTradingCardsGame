namespace MonsterTradingCardsGame.DTOs;

public class BattleDTO {
    public string Username1 { get; set; } = "";
    public string Username2 { get; set; } = "";
    public string BattleLog { get; set; } = "";
    public bool Status1 { get; set; }
    public bool Status2 { get; set; }
    
    public override string ToString() {
        if(Status2 == false && Status1 == false) {
            return $"{Username1} vs. {Username2} ended in a draw\nBattlelog: \n{BattleLog}\n";
        }

        if (Status1) {
            return $"{Username1} won against {Username2}\nBattlelog: \n{BattleLog}\n";
        }
        
        return $"{Username2} won against {Username1}\nBattlelog: \n{BattleLog}\n";
    }
}