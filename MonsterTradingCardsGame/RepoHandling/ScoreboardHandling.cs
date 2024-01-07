using System.Net;
using System.Net.Mime;
using System.Text.Json;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;

namespace MonsterTradingCardsGame.RepoHandling;

public class ScoreboardHandling : IHTTPEndpoint{
    public void HandleRequest(HTTPRequest rq, HTTPResponse rs) {
        if (rq.Method == HTTPMethod.GET) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);
                var stats = unit.UserRepository.GetScoreboard();
                var body = JsonSerializer.Serialize(stats);
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
    }
}