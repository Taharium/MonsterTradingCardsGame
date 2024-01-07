using MonsterTradingCardsGame.RepoHandling;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace MonsterTradingCardsGame.MTCGServer;

public class Server {

    private readonly TcpListener _tcpListener;
    public ConcurrentDictionary<string, IHTTPEndpoint> Endpoints { get; private set; } = new ConcurrentDictionary<string, IHTTPEndpoint>();
    public Server(IPAddress ip, int port) {
        _tcpListener = new TcpListener(ip, port);
    }

    public void StartServer() {
        RegisterEndpoint();
        _tcpListener.Start();
        while (true) {
            // ----- 0. Accept the TCP-Client and create the reader and writer -----
            var clientSocket = _tcpListener.AcceptTcpClient();
            var httpProcessor = new ClientProcessor(this, clientSocket);
            // Use ThreadPool to make it multi-threaded
            ThreadPool.QueueUserWorkItem(_ => httpProcessor.Process());
        }
        // ReSharper disable once FunctionNeverReturns
    }
    
    private void RegisterEndpoint() {
        Endpoints.TryAdd("history", new HistoryHandling());
        Endpoints.TryAdd("battles", new BattleHandling());
        Endpoints.TryAdd("tradings", new TradingHandling());
        Endpoints.TryAdd("scoreboard", new ScoreboardHandling());
        Endpoints.TryAdd("stats", new StatsHandling());
        Endpoints.TryAdd("deck", new DeckHandling());
        Endpoints.TryAdd("transactions", new TransactionHandling());
        Endpoints.TryAdd("packages", new PackageHandling());
        Endpoints.TryAdd("sessions", new SessionHandling());
        Endpoints.TryAdd("cards", new CardHandling());
        Endpoints.TryAdd("users", new UserHandling());
    }
}