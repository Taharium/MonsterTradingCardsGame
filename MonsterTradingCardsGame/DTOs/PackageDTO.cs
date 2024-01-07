namespace MonsterTradingCardsGame.DTOs;

public class PackageDTO {
    public List<CardDTO> Package { get; set; } = new List<CardDTO>();
    public int PackageId { get; set; }
}