using System.Net;
using System.Net.Mime;
using System.Text.Json;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;
using Npgsql;

namespace MonsterTradingCardsGame.RepoHandling;

public class TransactionHandling : IHTTPEndpoint {
    
    public void HandleRequest(HTTPRequest rq, HTTPResponse rs) {
        if (rq.Method == HTTPMethod.POST) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);
                PackageDTO package = unit.PackageRepository.GetPackage();
                unit.PackageRepository.UpdatePackage(package.PackageId, username);
                unit.UserRepository.UpdateCoinsLost(username);
                unit.StackRepository.InsertIntoStack(username, package.Package);
                var body = JsonSerializer.Serialize(package);
                rs.Prepare(HttpStatusCode.OK, body, MediaTypeNames.Application.Json);
                unit.Commit();
            }
            catch (ProcessException e) {
                rs.ReturnErrorWithMessage(e.ErrorCode, e.ErrorMessage);
            }
            catch (PostgresException) {
                rs.ReturnErrorWithMessage(HttpStatusCode.Forbidden, "Not enough money for buying a card package\n"); 
            }
            catch (Exception) {
                rs.CheckReturnCode(500);
            }
        }
    }
}