using System.Text.Json;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using MonsterTradingCardsGame.DTOs;
using NUnit.Framework;

namespace MonsterTradingCardsGame.Test;

public class UpdateDeckTest {

    [Test]
    public void TestJsonDeserializeInUpdate() {
        string json =
            "[\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\"]";
        
        List<CardIdDTO> cardIds = new List<CardIdDTO>();
        var test = JsonSerializer.Deserialize<string[]>(json) ?? throw new Exception();
        foreach (var id in test) {
            cardIds.Add(new CardIdDTO {
                Id = id
            });
        }

        if (cardIds.Count != 4)
            throw new Exception();
        
        Assert.Multiple(() => {
            Assert.That(cardIds?[0].Id, Is.EqualTo("845f0dc7-37d0-426e-994e-43fc3ac83c08"));
            Assert.That(cardIds?[1].Id, Is.EqualTo("99f8f8dc-e25e-4a95-aa2c-782823f36e2a"));
            Assert.That(cardIds?[2].Id, Is.EqualTo("e85e3976-7c86-4d06-9a80-641c2019a79f"));
            Assert.That(cardIds?[3].Id, Is.EqualTo("171f6076-4eb5-4a7d-b3f2-2d650cc3d237"));
        });
    }
}