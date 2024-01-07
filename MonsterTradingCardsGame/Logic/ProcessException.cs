using System.Net;

namespace MonsterTradingCardsGame.Logic;

internal class ProcessException : Exception {
    public HttpStatusCode ErrorCode { get; set; }
    public int ErrorCodeInt { get; set; }
    public string ErrorMessage { get; set; } = null!;
    public ProcessException(HttpStatusCode errorCode) {
        ErrorCode = errorCode;
        ErrorCodeInt = (int)ErrorCode;
    }
        
    public ProcessException(HttpStatusCode errorCode, string message) {
        ErrorCode = errorCode;
        ErrorCodeInt = (int)ErrorCode;
        ErrorMessage = message;
    }
}