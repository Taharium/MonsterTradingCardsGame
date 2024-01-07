using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Enums;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Test;

public class BattleLogicTest {
    
    [SetUp]
    public void Setup() {
        EnumExtension.ClearDictionaries();
        EnumExtension.FillDictionaries();
    }

    [Test]
    public void OnlyMonsterBattleTest() {
        Player player1 = new Player() {
            Username = "testUser1"
        };
        Player player2 = new Player() {
            Username = "testUser2"
        };
        
        player1.SelectedCard = new Card(new CardDTO() {
            Name = "Dragon",
            Damage = 10f,
            Id = "2"
        });
        player2.SelectedCard = new Card(new CardDTO() {
            Name = "WaterGoblin",
            Damage = 10f,
            Id = "1"
        });
        
        player1.AddCard(player1.SelectedCard);
        player2.AddCard(player2.SelectedCard);
        
        BattleLogic.OnlyMonstersBattle(player1, player2);
        
        Assert.That(player1.SelectedCard.Damage, Is.EqualTo(10f));
        Assert.That(player2.SelectedCard.Damage, Is.EqualTo(10f));
        
        Assert.That(player2.DeckCount(), Is.EqualTo(0));
        Assert.That(player1.DeckCount(), Is.EqualTo(2));
    }

    [Test]
    public void OnlySpellBattleTest() {
        Player player1 = new Player() {
            Username = "testUser1"
        };
        Player player2 = new Player() {
            Username = "testUser2"
        };
        
        player1.SelectedCard = new Card(new CardDTO() {
            Name = "FireSpell",
            Damage = 10f,
            Id = "2"
        });
        player2.SelectedCard = new Card(new CardDTO() {
            Name = "IceSpell",
            Damage = 10f,
            Id = "1"
        });
        
        player1.AddCard(player1.SelectedCard);
        player2.AddCard(player2.SelectedCard);
        
        BattleLogic.OnlySpellBattle(player1, player2);
        
        Assert.That(Calc.Player1Damage, Is.EqualTo(2f));
        Assert.That(Calc.Player2Damage, Is.EqualTo(0.5f));
        
        Assert.That(player1.NewDamage, Is.EqualTo(player1.SelectedCard.Damage * Calc.Player1Damage));
        Assert.That(player2.NewDamage, Is.EqualTo(player2.SelectedCard.Damage * Calc.Player2Damage));
        
        Assert.That(player1.DeckCount(), Is.EqualTo(2));
        Assert.That(player2.DeckCount(), Is.EqualTo(0));
    }
    
    [Test]
    public void OnlySpellBattleTest2() {
        Player player1 = new Player() {
            Username = "testUser1"
        };
        Player player2 = new Player() {
            Username = "testUser2"
        };
        
        player1.SelectedCard = new Card(new CardDTO() {
            Name = "GroundSpell",
            Damage = 10f,
            Id = "2"
        });
        player2.SelectedCard = new Card(new CardDTO() {
            Name = "FireSpell",
            Damage = 10f,
            Id = "1"
        });
        
        player1.AddCard(player1.SelectedCard);
        player2.AddCard(player2.SelectedCard);
        
        BattleLogic.OnlySpellBattle(player1, player2);
        
        Assert.That(Calc.Player1Damage, Is.EqualTo(2f));
        Assert.That(Calc.Player2Damage, Is.EqualTo(0.5f));
        
        Assert.That(player1.NewDamage, Is.EqualTo(player1.SelectedCard.Damage * Calc.Player1Damage));
        Assert.That(player2.NewDamage, Is.EqualTo(player2.SelectedCard.Damage * Calc.Player2Damage));
        
        Assert.That(player1.DeckCount(), Is.EqualTo(2));
        Assert.That(player2.DeckCount(), Is.EqualTo(0));
    }
    
    [Test]
    public void MixedBattleTestKnight() {
        Player player1 = new Player() {
            Username = "testUser1"
        };
        Player player2 = new Player() {
            Username = "testUser2"
        };
        
        player1.SelectedCard = new Card(new CardDTO() {
            Name = "WaterSpell",
            Damage = 10f,
            Id = "2"
        });
        player2.SelectedCard = new Card(new CardDTO() {
            Name = "Knight",
            Damage = 10f,
            Id = "1"
        });
        
        player1.AddCard(player1.SelectedCard);
        player2.AddCard(player2.SelectedCard);
        
        BattleLogic.MonsterAndSpellBattle(player1, player2);
        
        Assert.That(Calc.Player1Damage, Is.EqualTo(1));
        Assert.That(Calc.Player2Damage, Is.EqualTo(1));
        
        Assert.That(player1.DeckCount(), Is.EqualTo(2));
        Assert.That(player2.DeckCount(), Is.EqualTo(0));
    }
    
    [Test]
    public void MixedBattleTestKraken() {
        Player player1 = new Player() {
            Username = "testUser1"
        };
        Player player2 = new Player() {
            Username = "testUser2"
        };
        
        player1.SelectedCard = new Card(new CardDTO() {
            Name = "Kraken",
            Damage = 10f,
            Id = "2"
        });
        player2.SelectedCard = new Card(new CardDTO() {
            Name = "WaterSpell",
            Damage = 10f,
            Id = "1"
        });
        
        player1.AddCard(player1.SelectedCard);
        player2.AddCard(player2.SelectedCard);
        
        BattleLogic.MonsterAndSpellBattle(player1, player2);
        
        Assert.That(Calc.Player1Damage, Is.EqualTo(1));
        Assert.That(Calc.Player2Damage, Is.EqualTo(1));
        
        Assert.That(player1.DeckCount(), Is.EqualTo(2));
        Assert.That(player2.DeckCount(), Is.EqualTo(0));
    }
    
}