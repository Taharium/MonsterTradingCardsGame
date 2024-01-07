using System.Net;
using System.Net.Mime;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.MTCGServer;

namespace MonsterTradingCardsGame.RepoHandling;

public class BattleHandling : IHTTPEndpoint{
    public void HandleRequest(HTTPRequest rq, HTTPResponse rs) {
        
        if (rq.Method == HTTPMethod.POST) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);
                var deck = unit.DeckRepository.GetCardsFromDeck(username);
                if (deck.Cards.Count < 4)
                    throw new ProcessException(HttpStatusCode.Forbidden, "Not enough cards in deck to start a battle.\n");

                Player player = new Player(deck, username);
                var result = new ResultDTO();
                BattleLogic.EnterLobby(player, ref result);
                
                if (result.Player1.Won) {
                    unit.UserRepository.UpdateUserStats(result.Player1.Username, result.Player2.Username);
                    unit.StackRepository.UpdateStack(result.Player1, result.Player2.Username);
                    unit.DeckRepository.DeleteDeck(result.Player2.Username);
                    unit.UserRepository.UpdateCoinsWon(result.Player1.Username);
                }
                else if (result.Player2.Won) {
                    unit.UserRepository.UpdateUserStats(result.Player2.Username, result.Player1.Username);
                    unit.StackRepository.UpdateStack(result.Player2, result.Player1.Username);
                    unit.DeckRepository.DeleteDeck(result.Player1.Username);
                    unit.UserRepository.UpdateCoinsWon(result.Player2.Username);
                }
                
                if (result.Draw || result.Player1.Won || result.Player2.Won) {
                    unit.BattleRepository.CreateBattle(result);
                }
                rs.Prepare(HttpStatusCode.OK, result.BattleLog, MediaTypeNames.Text.Plain);
                unit.Commit();
            }
            catch (ProcessException e) {
                rs.ReturnErrorWithMessage(e.ErrorCode, e.ErrorMessage);
            }
            catch (Exception) {
                rs.CheckReturnCode(500);
            }
        }
    }
}