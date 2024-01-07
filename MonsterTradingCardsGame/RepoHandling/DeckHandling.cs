using System.Net;
using System.Net.Mime;
using System.Text.Json;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;

namespace MonsterTradingCardsGame.RepoHandling;

public class DeckHandling : IHTTPEndpoint {
    public void HandleRequest(HTTPRequest rq, HTTPResponse rs) {

        if (rq.Method == HTTPMethod.GET) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);
                var deck = unit.DeckRepository.GetCardsFromDeck(username);
                
                if (rq.QueryParameters.ContainsKey("format") && rq.QueryParameters["format"] == "plain") {
                    rs.Prepare(HttpStatusCode.OK, deck.ToCustomString(), MediaTypeNames.Text.Plain);
                    unit.Commit();
                    return;
                }
                var body = JsonSerializer.Serialize(deck.Cards) ?? throw new ProcessException(HttpStatusCode.InternalServerError, "");
                rs.Prepare(HttpStatusCode.OK, body, MediaTypeNames.Application.Json);
                unit.Commit();
            }
            catch (ProcessException e) {
                rs.ReturnErrorWithMessage(e.ErrorCode, e.ErrorMessage);
            }
            catch (Exception) {
                rs.CheckReturnCode(500);
            }
        }
        
        if (rq.Method == HTTPMethod.PUT) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);
                unit.DeckRepository.UpdateDeck(rq, username);
                rs.Prepare(HttpStatusCode.OK, "Deck sucessfully updated\n", MediaTypeNames.Text.Plain);
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