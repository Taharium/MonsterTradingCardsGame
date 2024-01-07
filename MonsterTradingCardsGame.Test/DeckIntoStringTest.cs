using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Test;

public class DeckIntoStringTest {

    [Test]
    public void TestDeckIntoString() {
        EnumExtension.FillDictionaries();
        var deck = new Deck();
        deck.Cards.Add(new Card("uifhdshf432423", "WaterGoblin", 10));
        deck.Cards.Add(new Card("2343242knkj2njknjb4hj2", "Dragon", 20));
        deck.Cards.Add(new Card("2343242knkj2njknjb4hj2", "Dragon", 20));
        
        string deckString = deck.ToCustomString();
        
        Assert.That(deckString, Is.EqualTo("[\n\tId: uifhdshf432423, Name: WaterGoblin, Damage: 10, Type: Monster, Species: Goblin, Element: Water\n\tId: 2343242knkj2njknjb4hj2, Name: Dragon, Damage: 20, Type: Monster, Species: Dragon, Element: Regular\n\tId: 2343242knkj2njknjb4hj2, Name: Dragon, Damage: 20, Type: Monster, Species: Dragon, Element: Regular\n]"));
    }
}