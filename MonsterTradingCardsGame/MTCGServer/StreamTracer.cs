namespace MonsterTradingCardsGame.MTCGServer;

internal class StreamTracer {
    private readonly StreamWriter _streamWriter;

    public StreamTracer(StreamWriter streamWriter) {
        _streamWriter = streamWriter;
    }

    internal void WriteLine(string v) { 
        Console.WriteLine(v);
        _streamWriter.WriteLine(v);
    }

    internal void WriteLine() {
        Console.WriteLine();
        _streamWriter.WriteLine();
    }
}