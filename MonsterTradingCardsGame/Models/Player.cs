namespace MonsterTradingCardsGame.Models;

public class Player {
    public string Username { get; set; } = "";
    public Deck PlayerDeck { get; set; } = new Deck();
    public Card SelectedCard { get; set; } = new Card();
    public bool Won { get; set; }
    public float NewDamage { get; set; }

    public Player(Deck deck) {
        PlayerDeck = deck;
    }

    public Player(Deck deck, string username) {
        PlayerDeck = deck;
        Username = username;
    }
    
    public Player() {}
    
    public void AddCard(Card card) {
        PlayerDeck.Cards.Add(card);
    }
    
    public void RemoveCard(Card card) {
        PlayerDeck.Cards.Remove(card);
    }

    public int DeckCount() {
        return PlayerDeck.Cards.Count;
    }
    
    public void GetRandomCard() {
        List<Card> cards = PlayerDeck.Cards;
        Random random = new Random();
        SelectedCard = cards[random.Next(cards.Count)];
    }
}