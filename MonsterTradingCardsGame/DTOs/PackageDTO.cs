namespace MonsterTradingCardsGame.DTOs;

public class PackageDTO {
    public int PackageId { get; set; }
    public List<CardDTO> Package { get; set; } = new List<CardDTO>();
}