using System.Text;

namespace MonsterTradingCardsGame.MTCGServer;

public class HTTPRequest {
    private readonly StreamReader _reader;

    public HTTPMethod Method { get; private set; } = HTTPMethod.GET;
    public string[]? Path { get; private set; } = Array.Empty<string>();
    public Dictionary<string, string> Headers { get; set; } = new();
    public readonly Dictionary<string, string> QueryParameters = new();
    public string HttpVersion { get; private set; } = "";
    public string? Content { get; private set; }

    /*public override string ToString()
    {
        return $"Person: Method={Method}, HttpVersion={HttpVersion}, Content={Content}\n";
    }*/

    public void PrintAll() {
        Console.WriteLine($"\nHTTPRequest: Method={Method}, HttpVersion={HttpVersion}, Content={Content}\n");

        if (Path is null)
            return;

        for (int i = 0; i < Path.Length; i++)
            Console.WriteLine($"Path: Path[{i}] = {Path[i]}\n");

        foreach (var pair in Headers) 
            Console.WriteLine($"\nHeaders: Key: {pair.Key}, Value: {pair.Value}");

        foreach (var pair in QueryParameters)
            Console.WriteLine($"\nQueryParameters: Key: {pair.Key}, Value: {pair.Value}");

        Console.WriteLine("\nEnd: HTTPRequest\n");
    }


    public HTTPRequest(StreamReader reader) {
        _reader = reader;
    }

    public void ParseRequest() {
        // 1.1 first line in HTTP contains the method, path and HTTP version
        string? line = _reader.ReadLine();

        Console.WriteLine(line);

        string[]? firstLineParts = line?.Split(' ');
        Method = (HTTPMethod)Enum.Parse(typeof(HTTPMethod), firstLineParts?[0] ?? "GET");
        string[] pathAndQuery = firstLineParts?[1].Split('?') ?? Array.Empty<string>();
        Path = pathAndQuery[0].Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (pathAndQuery.Length > 1) {
            string[] queryParams = pathAndQuery[1].Split('&');
            foreach (var queryParam in queryParams) {
                string[] queryParamParts = queryParam.Split('=');

                if (queryParamParts.Length >= 1)
                    QueryParameters[queryParamParts[0]] = (queryParamParts.Length == 2) ? queryParamParts[1] : "";
            }
        }
        HttpVersion = firstLineParts?[2] ?? "";

        // 1.2 read the HTTP-headers (in HTTP after the first line, until the empy line)
        int contentLength = 0; // we need the content_length later, to be able to read the HTTP-content
        while ((line = _reader.ReadLine()) != null) {
            Console.WriteLine(line);
            if (line == "")
                break;  // empty line indicates the end of the HTTP-headers

            // Parse the header
            string[] parts = line.Split(':');
            if (parts.Length == 2) {
                Headers[parts[0]] = parts[1].Trim();
                if (parts[0] == "Content-Length") {
                    contentLength = int.Parse(parts[1].Trim());
                }
            }
        }

        // 1.3 read the body if existing
        if (contentLength > 0 && Headers.ContainsKey("Content-Type")) {
            var data = new StringBuilder(200);
            char[] chars = new char[1024];
            int bytesReadTotal = 0;

            while (bytesReadTotal < contentLength) {
                try {
                    var bytesRead = _reader.Read(chars, 0, 1024);
                    bytesReadTotal += bytesRead;
                    if (bytesRead == 0) break;
                    data.Append(chars, 0, bytesRead);
                }
                // IOException can occur when there is a mismatch of the 'Content-Length'
                // because a different encoding is used
                // Sending a 'plain/text' payload with special characters (äüö...) is
                // an example of this
                catch (IOException) { break; }
                catch (Exception) { break; }
            }
            Content = data.ToString();
            Console.WriteLine(data.ToString());
        }
    }
}