using System.Net;
using System.Net.Mime;
using System.Text.Json;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;

namespace MonsterTradingCardsGame.RepoHandling;

public class TradingHandling : IHTTPEndpoint{
    public void HandleRequest(HTTPRequest rq, HTTPResponse rs) {
        
        if (rq.Method == HTTPMethod.GET) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);
                var tradingDeals = unit.TradingRepository.GetTradingDeals(username);
                if (tradingDeals.Count == 0) {
                    rs.Prepare(HttpStatusCode.Accepted, "No Trading Deals found\n", MediaTypeNames.Text.Plain);
                    unit.Commit();
                    return;
                }
                var body = JsonSerializer.Serialize(tradingDeals) ?? throw new ProcessException(HttpStatusCode.InternalServerError, "");
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
        
        if (rq.Method == HTTPMethod.POST) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);

                if (rq.Path?.Length == 2) {
                    unit.TradingRepository.TradeCards(rq, username);
                    rs.Prepare(HttpStatusCode.Created,"Trading deal successfully executed\n", MediaTypeNames.Text.Plain);
                    unit.Commit();
                    return;
                }
                unit.TradingRepository.CreateTradingDeal(rq, username);
                rs.Prepare(HttpStatusCode.OK, "Trading deal successfully created\n", MediaTypeNames.Text.Plain);
                unit.Commit();
            }
            catch (ProcessException e) {
                rs.ReturnErrorWithMessage(e.ErrorCode, e.ErrorMessage);
            }
            catch (Exception) {
                rs.CheckReturnCode(500);
            }
        }

        if (rq.Method == HTTPMethod.DELETE) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);
                if(rq.Path?.Length != 2) 
                    throw new ProcessException(HttpStatusCode.BadRequest, "Path is null");
                unit.TradingRepository.DeleteTradingDeal(rq.Path[1], username);
                rs.Prepare(HttpStatusCode.OK, "Trading deal successfully deleted\n", MediaTypeNames.Text.Plain);
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