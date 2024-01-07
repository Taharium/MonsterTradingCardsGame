using System.Net;
using System.Net.Mime;

namespace MonsterTradingCardsGame.MTCGServer;

public class HTTPResponse {
    public readonly StreamWriter _writer;

    public string HttpVersion { get; set; } = "HTTP/1.1";
    public int ReturnCode { get; set; } = (int)HttpStatusCode.OK;
    public HttpStatusCode ReturnMessage { get; set; } = HttpStatusCode.OK;
    public Dictionary<string, string> Headers { get; set; } = new();
    public string? Content { get; set; } = "";

    public string Type { get; set; } = "";

    /*public override string ToString()
    {
        return $"Person: ReturnCode={ReturnCode}, ReturnMessage={ReturnMessage}, HttpVersion={HttpVersion}, Content={Content}\n";
    }*/

    public void Printall() {
        Console.WriteLine($"\nHTTPResponse: ReturnCode={ReturnCode}, ReturnMessage={ReturnMessage}, HttpVersion={HttpVersion}, Content={Content}\n");
        foreach (var pair in Headers) {
            Console.WriteLine($"\nHeaders: Key: {pair.Key}, Value: {pair.Value}");
        }

        Console.WriteLine("\nEnd: HTTPResponse\n");
    }

    public HTTPResponse(StreamWriter writer) {
        _writer = writer;
    }

    public void Prepare(HttpStatusCode statusCode, string? content, string type) {
        ReturnCode = (int)statusCode;
        ReturnMessage = statusCode;
        Content = content;
        Type = type;
            
        if(type.Contains("plain"))
            Type = MediaTypeNames.Text.Plain;
            
        if (Content is { Length: > 0 }) {
            Headers["Content-Length"] = Content.Length.ToString();
            Headers["Content-Type"] = Type;
        }
        else
            Headers["Content-Length"] = "0";
    }
        
    public void ReturnErrorWithMessage(HttpStatusCode statusCode, string message) {
        if(statusCode == HttpStatusCode.InternalServerError)
            CheckReturnCode(500);
        else
            Prepare(statusCode, message, MediaTypeNames.Text.Plain);
    }
        
    public void CheckReturnCode(int returnCode) {
        switch (returnCode) {
            case 404:
                Prepare(HttpStatusCode.NotFound, "<html><body>Not found!</body></html>", MediaTypeNames.Text.Html);
                break;
            case 403:
                Prepare(HttpStatusCode.Forbidden, "<html><body>Forbidden!</body></html>", MediaTypeNames.Text.Html);
                break;
            case 409:
                Prepare(HttpStatusCode.Conflict, "<html><body>Conflict!</body></html>", MediaTypeNames.Text.Html);
                break;
            /*case 502:
                Prepare(HttpStatusCode.BadGateway, "<html><body>Bad Gateway!</body></html>", MediaTypeNames.Text.Html);
                break;*/
            case 500:
                Prepare(HttpStatusCode.InternalServerError, "<html><body>Internal Server Error!</body></html>", MediaTypeNames.Text.Html);
                break;
            default:
                Prepare(HttpStatusCode.BadRequest, "<html><body>Bad Request!</body></html>", MediaTypeNames.Text.Html);
                break;
        }
    }

    public void SendResponse() {
        var writerAlsoToConsole = new StreamTracer(_writer);  // we use a simple helper-class StreamTracer to write the HTTP-Response to the client and to the console

        writerAlsoToConsole.WriteLine($"{HttpVersion} {ReturnCode} {ReturnMessage}");

        foreach (var header in Headers)
            writerAlsoToConsole.WriteLine($"{header.Key}: {header.Value}");

        writerAlsoToConsole.WriteLine();

        if (Content != null)
            writerAlsoToConsole.WriteLine($"{Content}");
    }
}