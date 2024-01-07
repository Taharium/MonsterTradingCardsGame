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
                if(rq.QueryParameters.ContainsKey("detailed") && rq.QueryParameters["detailed"] == "true" && rq.QueryParameters.ContainsKey("format") && rq.QueryParameters["format"] == "plain") {
                    var historyDetailed = unit.TradingRepository.GetTradingHistoryDetailed(username).ToCustomString();
                    rs.Prepare(HttpStatusCode.OK, historyDetailed, MediaTypeNames.Text.Plain);
                    unit.Commit();
                    return;
                }
                if(rq.QueryParameters.ContainsKey("detailed") && rq.QueryParameters["detailed"] == "true") {
                    var historyDetailed = unit.TradingRepository.GetTradingHistoryDetailed(username).Beautify();
                    rs.Prepare(HttpStatusCode.OK, historyDetailed, MediaTypeNames.Text.Plain);
                    unit.Commit();
                    return;
                }
                var history = unit.TradingRepository.GetTradingHistory(username).Beautify();
                rs.Prepare(HttpStatusCode.OK, history, MediaTypeNames.Application.Json);
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
                var history = unit.BattleRepository.GetBattleHistory(username).Beautify();
                rs.Prepare(HttpStatusCode.OK, history, MediaTypeNames.Application.Json);
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