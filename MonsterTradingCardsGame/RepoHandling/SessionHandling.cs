using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.MTCGServer;

namespace MonsterTradingCardsGame.RepoHandling;

public class SessionHandling : IHTTPEndpoint {
    public void HandleRequest(HTTPRequest rq, HTTPResponse rs) {
        if (rq.Method == HTTPMethod.POST) {
            using UnitOfWork unit = new UnitOfWork();
            try {
                unit.SessionRepository.CreateSession(rq, rs);
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