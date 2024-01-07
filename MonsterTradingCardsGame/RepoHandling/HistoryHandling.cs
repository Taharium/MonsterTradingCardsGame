using System.Net;
using System.Net.Mime;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;

namespace MonsterTradingCardsGame.RepoHandling;

public class HistoryHandling : IHTTPEndpoint {
    public void HandleRequest(HTTPRequest rq, HTTPResponse rs) {
        if (rq is { Method: HTTPMethod.GET, Path: [_, "tradings"] }) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);
                if(rq.QueryParameters.ContainsKey("detailed") && rq.QueryParameters["detailed"] == "true") {
                    var historyDetailed = unit.TradingRepository.GetTradingHistoryDetailed(username);
                    rs.Prepare(HttpStatusCode.OK, historyDetailed.ToCustomString(), MediaTypeNames.Text.Plain);
                    unit.Commit();
                    return;
                }
                var history = unit.TradingRepository.GetTradingHistory(username);
                rs.Prepare(HttpStatusCode.OK, history.ToCustomString(), "text/plain");
                unit.Commit();
            }
            catch (ProcessException e) {
                rs.ReturnErrorWithMessage(e.ErrorCode, e.ErrorMessage);
            }
            catch (Exception) {
                rs.CheckReturnCode(500);
            }
        }
        
        if(rq is { Method: HTTPMethod.GET, Path: [_, "battles"] }) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);
                var history = unit.BattleRepository.GetBattleHistory(username);
                rs.Prepare(HttpStatusCode.OK, history.ToCustomString(), MediaTypeNames.Text.Plain);
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