using System.Text.Json;
using MonsterTradingCardsGame.Enums;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.MTCGServer;
using MonsterTradingCardsGame.Repository;

namespace MonsterTradingCardsGame.Test;

public class JsonSerializerCardTest {
    [Test]
    public void TestJsonSerializerCard() {
        EnumExtension.FillDictionaries();
        //string c = "[{\"Id\":\"67f9048f-99b8-4ae4-b866-d8008d00c53d\", \"Name\":\"WaterGoblin\", \"Damage\": 10.0}, {\"Id\":\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"Name\":\"RegularSpell\", \"Damage\": 50.0}, {\"Id\":\"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"Name\":\"Knight\", \"Damage\": 20.0}, {\"Id\":\"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\", \"Name\":\"RegularSpell\", \"Damage\": 45.0}, {\"Id\":\"2508bf5c-20d7-43b4-8c77-bc677decadef\", \"Name\":\"FireElf\", \"Damage\": 25.0}]";
        string a = "{\"Id\":\"dfdd758f-649c-40f9-ba3a-8657f4b3439f\", \"Name\":\"FireSpell\",    \"Damage\": 25.0}";
        //PackageRepository repository = new PackageRepository();
        
        Card card = JsonSerializer.Deserialize<Card>(a) ?? throw new Exception();
        Assert.That(card, Is.Not.Null);
        Assert.That(card.Id, Is.EqualTo("dfdd758f-649c-40f9-ba3a-8657f4b3439f"));
        Assert.That(card.Name, Is.EqualTo(CardName.FireSpell));
        Assert.That(card.Damage, Is.EqualTo(25.0));
        
        card.SetProperties(card.Name.ToString());
        
        Assert.That(card.ElementType, Is.EqualTo(CardElementType.Fire));
        Assert.That(card.Type, Is.EqualTo(CardType.Spell));
        Assert.That(card.Species, Is.EqualTo(CardSpecies.Spell));
    }
}