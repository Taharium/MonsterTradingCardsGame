using System.Net;
using System.Net.Mime;
using System.Text.Json;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;

namespace MonsterTradingCardsGame.RepoHandling;

public class StatsHandling : IHTTPEndpoint {
    public void HandleRequest(HTTPRequest rq, HTTPResponse rs) {
        if (rq.Method == HTTPMethod.GET) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);
                if (rq.QueryParameters.ContainsKey("ratio") && rq.QueryParameters["ratio"] == "true") {
                    var statsRatio = unit.UserRepository.GetStatsRatio(username);
                    rs.Prepare(HttpStatusCode.OK, statsRatio.WinRatio(), MediaTypeNames.Text.Plain);
                    unit.Commit();
                    return;
                }
                var stats = unit.UserRepository.GetStats(username).Beautify();
                rs.Prepare(HttpStatusCode.OK, stats, MediaTypeNames.Application.Json);
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
