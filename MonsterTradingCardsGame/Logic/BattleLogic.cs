using System.Text;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Enums;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Logic;

public static class Calc {
    public static float Player1Damage = 1f;
    public static float Player2Damage = 1f;
}

public static class BattleLogic {
    private static int _round = 0;
    private static readonly object LockQueue = new();
    private static Queue<Player> _queue = new();
    private static StringBuilder _battleLog = new();
    
    private static ResultDTO StartBattle(Player player1, Player player2) {
        _battleLog.Append("Battle starts\n");
        _battleLog.Append($"Player1: {player1.Username}\n");
        _battleLog.Append($"Player2: {player2.Username}\n");
        while (_round != 100 && player1.DeckCount() != 0 && player2.DeckCount() != 0) {
            _round++;
            _battleLog.Append($"Round {_round}\n"); 
            player1.GetRandomCard();
            player2.GetRandomCard();
            _battleLog.Append($"Player1 card: {player1.SelectedCard.ToCustomString()}\n");
            _battleLog.Append($"Player2 card: {player2.SelectedCard.ToCustomString()}\n");
            if (player1.SelectedCard.Type == CardType.Monster && player2.SelectedCard.Type == CardType.Monster) {
                OnlyMonstersBattle(player1, player2);
            } else if (player1.SelectedCard.Type == CardType.Monster && player2.SelectedCard.Type == CardType.Spell) {
                MonsterAndSpellBattle(player1, player2);
            } else if (player1.SelectedCard.Type == CardType.Spell && player2.SelectedCard.Type == CardType.Monster) {
                MonsterAndSpellBattle(player1, player2);
            } else if (player1.SelectedCard.Type == CardType.Spell && player2.SelectedCard.Type == CardType.Spell) {
                OnlySpellBattle(player1, player2);
            }
        }

        if (player1.DeckCount() == 0) {
            _battleLog.Append($"Player2 ({player2.Username}) wins the Game in {_round} rounds: Player1 ({player1.Username}) has no cards left\n\n");
            player2.Won = true;
            var battleLog = _battleLog.ToString();
            ResetBattle();
            return new ResultDTO() {
                BattleLog = battleLog,
                Player1 = player1,
                Player2 = player2,
            };
        }
        
        if (player2.DeckCount() == 0) {
            _battleLog.Append($"Player1 ({player1.Username}) wins the Game in {_round} rounds: Player2 ({player2.Username}) has no cards left\n\n");
            player1.Won = true;
            var battleLog = _battleLog.ToString();
            ResetBattle();
            return new ResultDTO() {
                BattleLog = battleLog,
                Player1 = player1,
                Player2 = player2
            };
        }
        
        if(_round == 100) {
            _battleLog.Append("Draw: 100 rounds have passed\n\n");
            var battleLog = _battleLog.ToString();
            ResetBattle();
            return new ResultDTO() {
                BattleLog = battleLog,
                Draw = true
            };
        }
        
        ResetBattle();
        return new ResultDTO() {
            BattleLog = "The Battle unexpectedly ended\n\n"
        };
    }

    private static void Battle(Player player1, Player player2) {
        if (player1.NewDamage > player2.NewDamage) {
            _battleLog.Append($"Player1 wins: Player1's CardDamage ({player1.NewDamage}) is greater than Player2's CardDamage ({player2.NewDamage})\n");
            player1.AddCard(player2.SelectedCard);
            player2.RemoveCard(player2.SelectedCard);
        } else if (player1.NewDamage < player2.NewDamage) {
            _battleLog.Append($"Player2 wins: Player2's CardDamage ({player2.NewDamage}) is greater than Player1's CardDamage ({player1.NewDamage})\n");
            player2.AddCard(player1.SelectedCard);
            player1.RemoveCard(player1.SelectedCard);
        } else {
            _battleLog.Append($"Draw Player1 and Player2's CardDamage are equal ({player1.NewDamage})\n");
        }
    }

    public static void OnlySpellBattle(Player player1, Player player2) {
        _battleLog.Append("Only Spell Battle\nStart Calculation\n");
        Rules.Calculation(player1, player2, _battleLog);
        //Rules.BurnedByFire(player1, player2, _battleLog);
        
        player1.NewDamage = player1.SelectedCard.Damage * Calc.Player1Damage;
        player2.NewDamage = player2.SelectedCard.Damage * Calc.Player2Damage;
        
        Battle(player1, player2);
    }

    public static void OnlyMonstersBattle(Player player1, Player player2) {
        _battleLog.Append("Only Monster Battle\n");
        if (Rules.MonsterTypeAdvantage(player1, player2, _battleLog))
            return;
        
        Battle(player1, player2);
    }
    
    public static void MonsterAndSpellBattle(Player player1, Player player2) {
        _battleLog.Append("Monster and Spell Battle\n");
        if(Rules.MonsterAgainstSpell(player1, player2, _battleLog))
            return;
        _battleLog.Append("Start Calculation\n");
        Rules.Calculation(player1, player2, _battleLog);
        //Rules.BurnedByFire(player1, player2, _battleLog);
        
        player1.NewDamage = player1.SelectedCard.Damage * Calc.Player1Damage;
        player2.NewDamage = player2.SelectedCard.Damage * Calc.Player2Damage;
        
        Battle(player1, player2);
    }
    
    private static void ResetBattle() {
        _round = 1;
        _battleLog.Clear();
    }

    public static void EnterLobby(Player player, ref ResultDTO result) {
        lock (LockQueue) {
            if(!_queue.Contains(player))
                _queue.Enqueue(player);
            
            if (_queue.Count == 2) {
                _battleLog.Append("Entered Lobby\n");
                _battleLog.Append("There are 2 players in the lobby\n");
                Player player1 = _queue.Dequeue();
                Player player2 = _queue.Dequeue();
                result = StartBattle(player1, player2);
            }
        }
    }
}