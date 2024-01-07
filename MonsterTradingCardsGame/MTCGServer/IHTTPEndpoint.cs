namespace MonsterTradingCardsGame.MTCGServer
{
    public interface IHTTPEndpoint {
        void HandleRequest(HTTPRequest rq, HTTPResponse rs);
    }
}
