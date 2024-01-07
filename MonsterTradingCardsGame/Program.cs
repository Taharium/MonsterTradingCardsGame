using MonsterTradingCardsGame.MTCGServer;
using System.Net;
using MonsterTradingCardsGame.Extensions;

namespace MonsterTradingCardsGame;

internal class Program {
    static void Main() {
        EnumExtension.FillDictionaries();

        Console.WriteLine("Starting Server...");
        Server server = new Server(IPAddress.Loopback, 10001);
        server.StartServer();
    }
}