using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Extensions;

namespace MonsterTradingCardsGame.Test;

public class ListExtensionTest {
    [Test]
    public void Test_ToCustomStringWithTradingHistoryDTO() {
        List<TradingHistoryDTO> tradingHistoryList = new List<TradingHistoryDTO> {
            new TradingHistoryDTO {
                Id = "1",
                Offerer = 123,
                CardToTrade = "Card1",
                Type = "Monster",
                MinimumDamage = 50.5f,
                Trader = 456,
                CardToReceive = "Card2"
            },
            new TradingHistoryDTO {
                Id = "2",
                Offerer = 789,
                CardToTrade = "Card3",
                Type = "Monster",
                MinimumDamage = 30.0f,
                Trader = 987,
                CardToReceive = "Card4"
            },
            // Add more dummy data as needed
        };   
        
        string expected = "[\n" +
                          "\tTradingId: 1; Offerer: 123; CardToTrade: Card1;\n\t\tType: Monster; MinimumDamage: 50.5; Trader: 456; CardToReceive: Card2,\n" +
                          "\tTradingId: 2; Offerer: 789; CardToTrade: Card3;\n\t\tType: Monster; MinimumDamage: 30; Trader: 987; CardToReceive: Card4,\n" +
                          "]";
        
        Assert.That(tradingHistoryList.ToCustomString(), Is.EqualTo(expected));
    }
    
    [Test]
    public void Test_ToCustomStringWithTradingHistoryDetailedDTO() {
        List<TradingHistoryDetailedDTO> tradingHistoryDetailedList = new List<TradingHistoryDetailedDTO> {
            new TradingHistoryDetailedDTO {
                Id = "1",
                Offerer = "User1",
                CardToTrade = "Card1",
                Type = "Monster",
                MinimumDamage = 50.5f,
                Trader = "User2",
                CardToReceive = "Card2",
                OffererCardName = "Card1",
                OffererCardType = "Monster",
                OffererCardDamage = 50.5f,
                TraderCardName = "Card2",
                TraderCardType = "Monster",
                TraderCardDamage = 50.5f
            },
            new TradingHistoryDetailedDTO {
                Id = "2",
                Offerer = "User3",
                CardToTrade = "Card3",
                Type = "Monster",
                MinimumDamage = 30.0f,
                Trader = "User4",
                CardToReceive = "Card4",
                OffererCardName = "Card3",
                OffererCardType = "Monster",
                OffererCardDamage = 30.0f,
                TraderCardName = "Card4",
                TraderCardType = "Monster",
                TraderCardDamage = 30.0f
            },
            // Add more dummy data as needed
        };   
        
        
        string expected = "[\n" +
                          "\tTradingId: 1; Type: Monster; MinimumDamage: 50.5;\n\t\tOfferer: User1; CardToTrade: Card1 => \n\t\t(OffererCardName: Card1, OffererCardType: Monster, OffererCardDamage: 50.5);\n\t\tTrader: User2; CardToReceive: Card2 => \n\t\t(TraderCardName: Card2, TraderCardType: Monster, TraderCardDamage: 50.5),\n" +
                          "\tTradingId: 2; Type: Monster; MinimumDamage: 30;\n\t\tOfferer: User3; CardToTrade: Card3 => \n\t\t(OffererCardName: Card3, OffererCardType: Monster, OffererCardDamage: 30);\n\t\tTrader: User4; CardToReceive: Card4 => \n\t\t(TraderCardName: Card4, TraderCardType: Monster, TraderCardDamage: 30),\n" +
                          "]";
        
        
        Assert.That(tradingHistoryDetailedList.ToCustomString(), Is.EqualTo(expected));
        
        
    }
}