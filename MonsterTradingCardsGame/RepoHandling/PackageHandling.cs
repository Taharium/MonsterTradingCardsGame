using System.Net;
using System.Net.Mime;
using System.Text.Json;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Extensions;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;

namespace MonsterTradingCardsGame.RepoHandling;

public class PackageHandling : IHTTPEndpoint {
    public void HandleRequest(HTTPRequest rq, HTTPResponse rs) {
        if (rq.Method == HTTPMethod.POST) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                var username = AuthToken.ParseTokenForUsername(rq.Headers["Authorization"]);
                unit.SessionRepository.CheckToken(username);
                
                if (rq.Content == null) {
                    throw new ProcessException(HttpStatusCode.InternalServerError, "");
                }                    
                var package = JsonSerializer.Deserialize<List<CardDTO>>(rq.Content);
                    
                if (package == null) {
                    throw new ProcessException(HttpStatusCode.InternalServerError, "");
                }

                unit.CardRepository.AddCards(package);
                unit.PackageRepository.CreatePackage(package);
                rs.Prepare(HttpStatusCode.Created, "Package and cards successfully created\n", MediaTypeNames.Text.Plain);
                unit.Commit();
            }
            catch(ProcessException e) {
                rs.ReturnErrorWithMessage(e.ErrorCode, e.ErrorMessage);
            }
            catch (Exception) {
                rs.CheckReturnCode(500);
            }
        }
    }
}