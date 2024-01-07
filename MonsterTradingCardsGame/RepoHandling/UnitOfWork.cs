using MonsterTradingCardsGame.Repository;
using Npgsql;

namespace MonsterTradingCardsGame.RepoHandling;

internal class UnitOfWork : IDisposable {
    private const string DbConnString = "Host=localhost;Username=postgres;Password=postgres;Database=mydb";
    private NpgsqlConnection Npg { get; }
    private readonly NpgsqlTransaction? _transaction;
    private bool _disposed;
    private bool _commited;
    private CardRepository? _cardRepository;
    private UserRepository? _userRepository;
    private PackageRepository? _packageRepository;
    private SessionRepository? _sessionRepository;
    private StackRepository? _stackRepository;
    private DeckRepository? _deckRepository;
    private TradingRepository? _tradingRepository;
    private BattleRepository? _battleRepository;

    public CardRepository CardRepository => _cardRepository ??= new CardRepository(Npg);
    public PackageRepository PackageRepository => _packageRepository ??= new PackageRepository(Npg);
    public UserRepository UserRepository => _userRepository ??= new UserRepository(Npg);
    public SessionRepository SessionRepository => _sessionRepository ??= new SessionRepository(Npg);
    public StackRepository StackRepository => _stackRepository ??= new StackRepository(Npg);
    public DeckRepository DeckRepository => _deckRepository ??= new DeckRepository(Npg);
    public TradingRepository TradingRepository => _tradingRepository ??= new TradingRepository(Npg);
    public BattleRepository BattleRepository => _battleRepository ??= new BattleRepository(Npg);
        
    public UnitOfWork() {
        Npg = new NpgsqlConnection(DbConnString);
        Npg.Open();
        _transaction = Npg.BeginTransaction();
    }

    public void Commit() {
        _commited = true;
        _transaction?.Commit();
    }

    public void Rollback() {
        _transaction?.Rollback();
    }

    private void Dispose(bool disposing) {
        if (!_disposed) {
            if (disposing) {
                if (!_commited)
                    _transaction?.Rollback();
                Npg.Close();
                Npg.Dispose();
            }
            _disposed = true;
            _commited = false;
        }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}