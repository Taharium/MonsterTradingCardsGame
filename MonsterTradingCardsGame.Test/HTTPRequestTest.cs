using MonsterTradingCardsGame.Enums;
using MonsterTradingCardsGame.MTCGServer;
using System.Text;

namespace MonsterTradingCardsGame.Test;

internal class HTTPRequestTest {
    [Test]
    public void HTTPRequest_CheckIfRequestParsedCorrectly() {
        string httpRequestData = "GET /path/to/resource?param1=value1&param2=value2 HTTP/1.1\r\n" +
                                 "Host: localhost\r\n" +
                                 "Content-Length: 10\r\n" +
                                 "Content-Type: text/plain\r\n" +
                                 "\r\n" +
                                 "HelloWorld";

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(httpRequestData));
        var reader = new StreamReader(stream);
        var rq = new HTTPRequest(reader);

        rq.ParseRequest();

        Assert.Multiple(() => {
            Assert.That(rq.Method, Is.EqualTo(HTTPMethod.GET));
            Assert.That(rq.Path, Is.EqualTo(new string[] { "path", "to", "resource" }));
            Assert.That(rq.QueryParameters["param1"], Is.EqualTo("value1"));
            Assert.That(rq.QueryParameters["param2"], Is.EqualTo("value2"));
            Assert.That(rq.HttpVersion, Is.EqualTo("HTTP/1.1"));
            Assert.That(rq.Content, Is.EqualTo("HelloWorld"));
            Assert.That(rq.Headers["Host"], Is.EqualTo("localhost"));
        });
    }

    [Test]
    public void ParseRequest_CheckIfRequestWithNoContentIsParsedCorrectly() {
        string httpRequestData = "GET /path/to/resource HTTP/1.1\r\n" +
                                 "Host: localhost\r\n" +
                                 "\r\n";

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(httpRequestData));
        var reader = new StreamReader(stream);
        var rq = new HTTPRequest(reader);

        rq.ParseRequest();

        Assert.Multiple(() => {
            Assert.That(rq.Method, Is.EqualTo(HTTPMethod.GET));
            Assert.That(rq.Path, Is.EqualTo(new string[] { "path", "to", "resource" }));
            Assert.That(rq.HttpVersion, Is.EqualTo("HTTP/1.1"));
            Assert.That(rq.Headers["Host"], Is.EqualTo("localhost"));
        });
        Assert.That(rq.Content, Is.Null);
    }

    [Test]
    public void ParseRequest_HandlesInvalidContentLength() {
        string httpRequestData = "POST /path/to/resource HTTP/1.1\r\n" +
                                 "Host: localhost\r\n" +
                                 "Content-Length: invalid\r\n" +
                                 "\r\n" +
                                 "InvalidContent";

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(httpRequestData));
        var reader = new StreamReader(stream);
        var rq = new HTTPRequest(reader);

        Assert.Throws<FormatException>(() => rq.ParseRequest());
        Assert.Multiple(() => {
            Assert.That(rq.Method, Is.EqualTo(HTTPMethod.POST));
            Assert.That(rq.Path, Is.EqualTo(new string[] { "path", "to", "resource" }));
            Assert.That(rq.HttpVersion, Is.EqualTo("HTTP/1.1"));
            Assert.That(rq.Headers["Host"], Is.EqualTo("localhost"));
        });
        Assert.That(rq.Content, Is.Null);
    }

    [Test]
    public void ParseRequest_HandlesMalformedRequest() {
        string malformedRequestData = "InvalidRequestData";

        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(malformedRequestData));
        var reader = new StreamReader(memoryStream);
        var httpRequest = new HTTPRequest(reader);

        Assert.Throws<ArgumentException>(httpRequest.ParseRequest);
        Assert.Multiple(() => {
            Assert.That(httpRequest.Method, Is.EqualTo(HTTPMethod.GET)); // Default method when parsing fails
            Assert.That(httpRequest.HttpVersion, Is.EqualTo("")); // Default version when parsing fails
        });
        Assert.Multiple(() => {
            Assert.That(httpRequest.Path, Is.Empty); // Default path when parsing fails
            Assert.That(httpRequest.Content, Is.Null);
            Assert.That(httpRequest.Headers, Is.Empty); // Empty headers dictionary when parsing fails
        });
    }
}