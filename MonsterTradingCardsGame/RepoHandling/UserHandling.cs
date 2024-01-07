using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;
using System.Net;
using System.Net.Mime;
using MonsterTradingCardsGame.DTOs;
using System.Text.Json;
using MonsterTradingCardsGame.Extensions;

namespace MonsterTradingCardsGame.RepoHandling;

public class UserHandling : IHTTPEndpoint {
    public void HandleRequest(HTTPRequest rq, HTTPResponse rs) {

        if (rq.Method == HTTPMethod.POST) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                unit.UserRepository.CreateUser(rq);
                rs.Prepare(HttpStatusCode.OK, "User successfully created\n", MediaTypeNames.Text.Plain);
                unit.Commit();
            }
            catch (ProcessException e) {
                rs.ReturnErrorWithMessage(e.ErrorCode, e.ErrorMessage);
            }
            catch (Exception) {
                rs.CheckReturnCode(500);
            }
        }
        
        if (rq.Method == HTTPMethod.GET) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                unit.SessionRepository.CheckTokenInHeader(rq);
                UserDataDTO user = unit.UserRepository.GetUserByUsername(rq);
                var body = user.Beautify();
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
                unit.SessionRepository.CheckTokenInHeader(rq);
                if (rq.Path is null) {
                    throw new ProcessException(HttpStatusCode.BadRequest, "Path is null");
                }
                var username = rq.Path[1];
                unit.UserRepository.UpdateNameBioImage(rq, username);
                rs.Prepare(HttpStatusCode.OK, "User sucessfully updated\n", MediaTypeNames.Text.Plain);
                unit.Commit();
            }
            catch (ProcessException e) {
                rs.ReturnErrorWithMessage(e.ErrorCode, e.ErrorMessage);
            }
            catch (Exception) {
                rs.CheckReturnCode(500);
            }
        }

        /*if (rq.Method == HTTPMethod.DELETE) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                return unit.UserRepository.Delete(rq, rs);
            }
            catch (ProcessException e) {
                rs.ReturnErrorWithMessage(e.ErrorCode, e.ErrorMessage);
            }
            catch (Exception) {
                rs.CheckReturnCode(500);
            }
        }*/
    }
}