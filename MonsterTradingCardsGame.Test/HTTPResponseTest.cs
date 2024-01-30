using MonsterTradingCardsGame.MTCGServer;
using System.Net;
using System.Net.Mime;

namespace MonsterTradingCardsGame.Test;

public class HTTPResponseTest {
    private const string body = "{'testing': 'test'}";

    [SetUp]
    public void Setup()
    {
    }

    /*[Test]
    public void HTTPResponse_CheckIfOutcomeIsTheSame()
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream) { AutoFlush = true };
        var rs = new HTTPResponse(writer)
        {
            Content = body,
            HttpVersion = "HTTP/1.1",
            ReturnCode = 200,
            ReturnMessage = HttpStatusCode.OK,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
            }
        };

        rs.SendResponse();
        stream.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(stream);
        var result = reader.ReadToEnd();

        string expectedOutput = "HTTP/1.1 200 OK\r\n" +
                                "Content-Length: 19\r\n" +
                                "Content-Type: application/json\r\n" +
                                "\r\n" +
                                "{'testing': 'test'}\r\n";

        Assert.That(expectedOutput, Is.EqualTo(result));
    }

    [Test]
    public void HTTPResponse_CheckBehaviourWithNull()
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream) { AutoFlush = true };
        var rs = new HTTPResponse(writer)
        {
            Content = null,
            HttpVersion = "HTTP/1.1",
            ReturnCode = 204,           //no content
            ReturnMessage = HttpStatusCode.NoContent,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }, //MediaTypeNames.Application.Json;
            }
        };

        rs.SendResponse();
        stream.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(stream);
        var result = reader.ReadToEnd();

        string expectedOutput = "HTTP/1.1 204 NoContent\r\n" +
                                "Content-Length: 0\r\n" +
                                "\r\n";

        Assert.That(expectedOutput, Is.EqualTo(result));
    }*/

    [Test]
    public void HTTPResponse_CheckFunctionPrepareWithContent() {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        writer.AutoFlush = true;
        var rs = new HTTPResponse(writer);
        rs.Prepare(HttpStatusCode.OK, body, MediaTypeNames.Application.Json);

        rs.SendResponse();

        stream.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(stream);
        var result = reader.ReadToEnd().Replace("\r","");

        string expectedOutput = "HTTP/1.1 200 OK\r\nContent-Length: 19\r\nContent-Type: application/json\r\n\r\n{'testing': 'test'}\r\n".Replace("\r","");

        Assert.That(expectedOutput, Is.EqualTo(result));
    }

    [Test]
    public void HTTPResponse_CheckFunctionPrepareWithoutContent() {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream) { AutoFlush = true };
        var rs = new HTTPResponse(writer);
        rs.Prepare(HttpStatusCode.NoContent, null, "");

        rs.SendResponse();
        stream.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(stream);
        var result = reader.ReadToEnd().Replace("\r","");

        string expectedOutput = "HTTP/1.1 204 NoContent\r\nContent-Length: 0\r\n\r\n".Replace("\r","");

        Assert.That(expectedOutput, Is.EqualTo(result));
    }
}