using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DTOs;

public class BattleDTO {
    [JsonIgnore]
    public string Username1 { get; set; } = "";
    [JsonIgnore]
    public string Username2 { get; set; } = "";
    public string Fight { get; set; } = "";
    public string Result { get; set; } = "";
    public string BattleLogShort { get; set; } = "";
    [JsonIgnore]
    public bool Status1 { get; set; }
    [JsonIgnore]
    public bool Status2 { get; set; }
    

    public void SetHistory() {
        Fight = $"{Username1} vs. {Username2}";
        if(Status2 == false && Status1 == false) {
            Result = "Draw";
            SetBattleLog();
            return;
        }

        if (Status1) {
            Result = $"{Username1} won";
            SetBattleLog();
            return;
        }
        
        if (Status2) {
            Result = $"{Username2} won";
        }
        SetBattleLog();
    }

    private void SetBattleLog() {
        string[] lines = BattleLogShort.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        if(lines.Length > 0)
            BattleLogShort = lines.Last();
    }
    
    public override string ToString() {
        if(Status2 == false && Status1 == false) {
            return $"{Username1} vs. {Username2} ended in a draw\nBattlelog: \n{BattleLogShort}\n";
        }

        if (Status1) {
            return $"{Username1} won against {Username2}\nBattlelog: \n{BattleLogShort}\n";
        }
        
        return $"{Username2} won against {Username1}\nBattlelog: \n{BattleLogShort}\n";
    }
}