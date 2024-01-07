using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Enums;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Test;

internal class CardTest {
    [Test]
    public void TestConstructor() {
        // Arrange
        EnumExtension.FillDictionaries();
        Card card = new Card("uifhdshf432423", "WaterGoblin", 10);
        Card card1 = new Card("2343242knkj2njknjb4hj2", "Dragon", 20);

        Assert.Multiple(() => {
            Assert.That(card.Id, Is.EqualTo("uifhdshf432423"));
            Assert.That(card.Name, Is.EqualTo(CardName.WaterGoblin));
            Assert.That(card.Damage, Is.EqualTo(10.0f));
            Assert.That(card.ElementType, Is.EqualTo(CardElementType.Water));
            Assert.That(card.Species, Is.EqualTo(CardSpecies.Goblin));
            Assert.That(card.Type, Is.EqualTo(CardType.Monster));
        });

        Assert.Multiple(() => {
            Assert.That(card1.Id, Is.EqualTo("2343242knkj2njknjb4hj2"));
            Assert.That(card1.Name, Is.EqualTo(CardName.Dragon));
            Assert.That(card1.Damage, Is.EqualTo(20.0f));
            Assert.That(card1.ElementType, Is.EqualTo(CardElementType.Regular));
            Assert.That(card1.Species, Is.EqualTo(CardSpecies.Dragon));
            Assert.That(card1.Type, Is.EqualTo(CardType.Monster));
        });
    }
    
    [Test]
    public void TestConstructorWithCardDTO() {
        // Arrange
        EnumExtension.FillDictionaries();
        CardDTO cardDTO = new CardDTO() {
            Id = "uifhdshf432423",
            Name = "Dragon",
            Damage = 10
        };
        Card card = new Card(cardDTO);
        
        string cardString = card.ToCustomString();
        
        Assert.Multiple(() => {
            Assert.That(card.Id, Is.EqualTo("uifhdshf432423"));
            Assert.That(card.Name, Is.EqualTo(CardName.Dragon));
            Assert.That(card.Damage, Is.EqualTo(10.0f));
            Assert.That(card.ElementType, Is.EqualTo(CardElementType.Regular));
            Assert.That(card.Species, Is.EqualTo(CardSpecies.Dragon));
            Assert.That(card.Type, Is.EqualTo(CardType.Monster));
        });
        
        Assert.That(cardString, Is.EqualTo("Id: uifhdshf432423, Name: Dragon, Damage: 10, Type: Monster, Species: Dragon, Element: Regular"));
    }
}