using System.Text;
using MonsterTradingCardsGame.Enums;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Logic;

public static class Rules {
    public static bool MonsterTypeAdvantage(Player player1, Player player2, StringBuilder battleLog) {
        if (player1.SelectedCard.Species == CardSpecies.Goblin && player2.SelectedCard.Species == CardSpecies.Dragon) {
            battleLog.Append("Player2 wins: Player2 has a Dragon and Player1 has a Goblin. Goblins fear Dragons and cannot attack. \n");
            player2.AddCard(player1.SelectedCard);
            player1.RemoveCard(player1.SelectedCard);
            return true;
        }
        if (player1.SelectedCard.Species == CardSpecies.Wizzard && player2.SelectedCard.Species == CardSpecies.Ork) {
            battleLog.Append("Player1 wins: Player2 has an Ork and Player1 has a Wizzard. Wizzards can controll Orks, therefor they cannot attck. \n");
            player1.AddCard(player2.SelectedCard);
            player2.RemoveCard(player2.SelectedCard);
            return true;
        } 
        if (player1.SelectedCard.Name == CardName.FireElf && player2.SelectedCard.Species == CardSpecies.Dragon) {
            battleLog.Append("Player1 wins: Player2 has a Dragon and Player1 has a FireElf. FireElfs can dodge attacks made by Dragons, which is why they cannot hit them. \n");
            player1.AddCard(player2.SelectedCard);
            player2.RemoveCard(player2.SelectedCard);
            return true;
        } 
        if (player2.SelectedCard.Species == CardSpecies.Goblin && player1.SelectedCard.Species == CardSpecies.Dragon) {
            battleLog.Append("Player1 wins: Player1 has a Dragon and Player2 has a Goblin. Goblins fear Dragons and cannot attack. \n");
            player1.AddCard(player2.SelectedCard);
            player2.RemoveCard(player2.SelectedCard);
            return true;
        }
        if (player2.SelectedCard.Species == CardSpecies.Wizzard && player1.SelectedCard.Species == CardSpecies.Ork) {
            battleLog.Append("Player2 wins: Player1 has an Ork and Player2 has a Wizzard. Wizzards can controll Orks, therefor they cannot attck. \n");
            player2.AddCard(player1.SelectedCard);
            player1.RemoveCard(player1.SelectedCard);
            return true;
        }
        if (player2.SelectedCard.Name == CardName.FireElf && player1.SelectedCard.Species == CardSpecies.Dragon) {
            battleLog.Append("Player2 wins: Player1 has a Dragon and Player2 has a FireElf. FireElfs can dodge attacks made by Dragons, which is why they cannot hit them. \n");
            player2.AddCard(player1.SelectedCard);
            player1.RemoveCard(player1.SelectedCard);
            return true;
        }
        return false;
    }
    
    public static void Calculation(Player player1, Player player2, StringBuilder battleLog) {
        switch (player1.SelectedCard.ElementType) {
            case CardElementType.Fire when player2.SelectedCard.ElementType == CardElementType.Water:
                Calc.Player1Damage = 0.5f;
                Calc.Player2Damage = 2f;
                battleLog.Append($"The Card of Player1 ({player1.SelectedCard.Name}) has an Element Advantage over the Card of Player2 ({player2.SelectedCard.Name})\nThe Card of Player1 does double damage and the Card of Player2 does half damage\n");
                break;
            case CardElementType.Regular when player2.SelectedCard.ElementType == CardElementType.Fire:
                Calc.Player1Damage = 0.5f;
                Calc.Player2Damage = 2f;
                battleLog.Append($"The Card of Player2 ({player2.SelectedCard.Name}) has an Element Advantage over the Card of Player1 ({player1.SelectedCard.Name})\nThe Card of Player2 does double damage and the Card of Player1 does half damage\n");
                break;
            case CardElementType.Water when player2.SelectedCard.ElementType == CardElementType.Regular:
                Calc.Player1Damage = 0.5f;
                Calc.Player2Damage = 2f;
                battleLog.Append($"The Card of Player2 ({player2.SelectedCard.Name}) has an Element Advantage over the Card of Player1 ({player1.SelectedCard.Name})\nThe Card of Player2 does double damage and the Card of Player1 does half damage\n");
                break;
            case CardElementType.Water when player2.SelectedCard.ElementType == CardElementType.Fire:
                Calc.Player1Damage = 2f;
                Calc.Player2Damage = 0.5f;
                battleLog.Append($"The Card of Player1 ({player1.SelectedCard.Name}) has an Element Advantage over the Card of Player2 ({player2.SelectedCard.Name})\nThe Card of Player1 does double damage and the Card of Player2 does half damage\n");
                break;
            case CardElementType.Fire when player2.SelectedCard.ElementType == CardElementType.Regular:
                Calc.Player1Damage = 2f;
                Calc.Player2Damage = 0.5f;
                battleLog.Append($"The Card of Player1 ({player1.SelectedCard.Name}) has an Element Advantage over the Card of Player2 ({player2.SelectedCard.Name})\nThe Card of Player1 does double damage and the Card of Player2 does half damage\n");
                break;
            case CardElementType.Regular when player2.SelectedCard.ElementType == CardElementType.Water:
                Calc.Player1Damage = 2f;
                Calc.Player2Damage = 0.5f;
                battleLog.Append($"The Card of Player1 ({player1.SelectedCard.Name}) has an Element Advantage over the Card of Player2 ({player2.SelectedCard.Name})\nThe Card of Player1 does double damage and the Card of Player2 does half damage\n");
                break;
            case CardElementType.Ice when player2.SelectedCard.ElementType == CardElementType.Ground:
                Calc.Player1Damage = 2f;
                Calc.Player2Damage = 0.5f;
                battleLog.Append($"The Card of Player1 ({player1.SelectedCard.Name}) has an Element Advantage over the Card of Player2 ({player2.SelectedCard.Name})\nThe Card of Player1 does double damage and the Card of Player2 does half damage\n");
                break;
            case CardElementType.Ground when player2.SelectedCard.ElementType == CardElementType.Ice:
                Calc.Player1Damage = 0.5f;
                Calc.Player2Damage = 2f;
                battleLog.Append($"The Card of Player2 ({player2.SelectedCard.Name}) has an Element Advantage over the Card of Player1 ({player1.SelectedCard.Name})\nThe Card of Player2 does double damage and the Card of Player1 does half damage\n");
                break;
            case CardElementType.Fire when player2.SelectedCard.ElementType == CardElementType.Ice:
                Calc.Player1Damage = 2f;
                Calc.Player2Damage = 0.5f;
                battleLog.Append($"The Card of Player1 ({player1.SelectedCard.Name}) has an Element Advantage over the Card of Player2 ({player2.SelectedCard.Name})\nThe Card of Player1 does double damage and the Card of Player2 does half damage\n");
                break;
            case CardElementType.Ice when player2.SelectedCard.ElementType == CardElementType.Fire:
                Calc.Player1Damage = 0.5f;
                Calc.Player2Damage = 2f;
                battleLog.Append($"The Card of Player2 ({player2.SelectedCard.Name}) has an Element Advantage over the Card of Player1 ({player1.SelectedCard.Name})\nThe Card of Player2 does double damage and the Card of Player1 does half damage\n");
                break;
            case CardElementType.Water when player2.SelectedCard.ElementType == CardElementType.Ground:
                Calc.Player1Damage = 2f;
                Calc.Player2Damage = 0.5f;
                battleLog.Append($"The Card of Player1 ({player1.SelectedCard.Name}) has an Element Advantage over the Card of Player2 ({player2.SelectedCard.Name})\nThe Card of Player1 does double damage and the Card of Player2 does half damage\n");
                break;
            case CardElementType.Ground when player2.SelectedCard.ElementType == CardElementType.Water:
                Calc.Player1Damage = 0.5f;
                Calc.Player2Damage = 2f;
                break;
            case CardElementType.Ground when player2.SelectedCard.ElementType == CardElementType.Fire:
                Calc.Player1Damage = 2f;
                Calc.Player2Damage = 0.5f;
                battleLog.Append($"The Card of Player1 ({player1.SelectedCard.Name}) has an Element Advantage over the Card of Player2 ({player2.SelectedCard.Name})\nThe Card of Player1 does double damage and the Card of Player2 does half damage\n");
                break;
            case CardElementType.Fire when player2.SelectedCard.ElementType == CardElementType.Ground:
                Calc.Player1Damage = 0.5f;
                Calc.Player2Damage = 2f;
                battleLog.Append($"The Card of Player2 ({player2.SelectedCard.Name}) has an Element Advantage over the Card of Player1 ({player1.SelectedCard.Name})\nThe Card of Player2 does double damage and the Card of Player1 does half damage\n");
                break;
            default:
                Calc.Player1Damage = 1f;
                Calc.Player2Damage = 1f;
                battleLog.Append("No Calculation needed. No Element Advantage\n");
                break;
        }
    }
    
    public static bool MonsterAgainstSpell(Player player1, Player player2, StringBuilder battleLog) {
        if (player1.SelectedCard.Name == CardName.Knight && player2.SelectedCard.Name == CardName.WaterSpell) {
            battleLog.Append("Player2 wins: Player1 has a Knight and Player2 has a WaterSpell. Knights are heavy and drown immediately. \n");
            player2.AddCard(player1.SelectedCard);
            player1.RemoveCard(player1.SelectedCard);
            return true;
        }
        if(player1.SelectedCard.Name == CardName.Kraken && player2.SelectedCard.Type == CardType.Spell) {
            battleLog.Append("Player1 wins: Player1 has a Kraken and Player2 has a Spell. Kraken are immune to spells. \n");
            player1.AddCard(player2.SelectedCard);
            player2.RemoveCard(player2.SelectedCard);
            return true;
        }
        if (player2.SelectedCard.Name == CardName.Knight && player1.SelectedCard.Name == CardName.WaterSpell) {
            battleLog.Append("Player1 wins: Player2 has a Knight and Player1 has a WaterSpell. Knights are heavy and drown immediately. \n");
            player1.AddCard(player2.SelectedCard);
            player2.RemoveCard(player2.SelectedCard);
            return true;
        }
        if(player2.SelectedCard.Name == CardName.Kraken && player1.SelectedCard.Type == CardType.Spell) {
            battleLog.Append("Player2 wins: Player2 has a Kraken and Player1 has a Spell. Kraken are immune to spells. \n");
            player2.AddCard(player1.SelectedCard);
            player1.RemoveCard(player1.SelectedCard);
            return true;
        }

        return false;
    }

    public static void BurnedByFire(Player player1, Player player2, StringBuilder battleLog) {
        var rnd = new Random();
        if(player1.SelectedCard.ElementType == CardElementType.Fire) {
            if(rnd.Next(1, 100) <= 30) {
                var rnd2 = new Random();
                var index = rnd2.Next(0, player2.DeckCount());
                player2.PlayerDeck.Cards[index].Damage -= 5;
                battleLog.Append("A random Card from Player2 was burned by Player1's Card\n. It lost 5 Damage\n");
            }
        }
        if(player2.SelectedCard.ElementType == CardElementType.Fire) {
            if(rnd.Next(1, 100) <= 30) {
                var rnd2 = new Random();
                var index = rnd2.Next(0, player1.DeckCount());
                player1.PlayerDeck.Cards[index].Damage -= 5;
                battleLog.Append("A random Card from Player1 was burned by Player2's Card\n. It lost 5 Damage\n");
            }
        }
    }
    
}