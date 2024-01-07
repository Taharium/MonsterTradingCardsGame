using System.Net.Sockets;

namespace MonsterTradingCardsGame.MTCGServer;

public class ClientProcessor {
    private readonly TcpClient _clientSocket;
    private readonly Server _httpServer;

    public ClientProcessor(Server httpServer, TcpClient clientSocket) {
        _httpServer = httpServer;
        _clientSocket = clientSocket;
    }

    public void Process() {

        // ----- 1. Read the HTTP-Request -----
        using var reader = new StreamReader(_clientSocket.GetStream());
        var rq = new HTTPRequest(reader);
        rq.ParseRequest();

        // ----- 2. Do the processing -----
        using var writer = new StreamWriter(_clientSocket.GetStream());
        writer.AutoFlush = true;
        var rs = new HTTPResponse(writer);

        if (rq.Path is null) {
            rs.CheckReturnCode(404);
            Send(rs, writer);
            return;
        }
            
        var endpoint = _httpServer.Endpoints.ContainsKey(rq.Path[0]) ? _httpServer.Endpoints[rq.Path[0]] : null;
            
        if (endpoint == null) {
            rs.CheckReturnCode(500);
            Send(rs, writer);
            return;
        }

        endpoint.HandleRequest(rq, rs);
        Send(rs, writer);
    }

    private void Send(HTTPResponse rs, StreamWriter writer) {
        Console.WriteLine("----------------------------------------");
        rs.SendResponse();
        writer.Flush();
        Console.WriteLine("========================================");
    }
}