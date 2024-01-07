using System.Net;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Logic;
using MonsterTradingCardsGame.Models;
using Npgsql;

namespace MonsterTradingCardsGame.Repository;

internal class CardRepository {
    private readonly NpgsqlConnection _npg;

    public CardRepository(NpgsqlConnection npg) {
        _npg = npg;
    }

    private void CheckCard(List<CardDTO> cards) {

        foreach (var c in cards) {
            using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM cards WHERE id = @id", _npg)) {
                cmd.Parameters.AddWithValue("id", c.Id);
                cmd.Prepare();

                if (cmd.ExecuteScalar() is not long result || result > 0)
                    throw new ProcessException(HttpStatusCode.Conflict, "A in the package included card already exists\n");
            
                return;
            }
        }

        throw new ProcessException(HttpStatusCode.FailedDependency, "Package contains no cards\n");
    }

    public void AddCards(List<CardDTO> cards) {
        lock (this) {
            CheckCard(cards);

            foreach (var card in cards.Select(c => new Card(c))) {
                using NpgsqlCommand command = new NpgsqlCommand("INSERT INTO cards (id, name, damage, element, species, cardtype) VALUES (@id, @name, @damage, @element, @species, @cardtype)", _npg);
                command.Parameters.AddWithValue("id", card.Id);
                command.Parameters.AddWithValue("name", card.Name.ToString());
                command.Parameters.AddWithValue("damage", card.Damage);
                command.Parameters.AddWithValue("element", card.ElementType.ToString());
                command.Parameters.AddWithValue("species", card.Species.ToString());
                command.Parameters.AddWithValue("cardtype", card.Type.ToString());
                command.Prepare();
            
                if (command.ExecuteNonQuery() != 1)
                    throw new ProcessException(HttpStatusCode.InternalServerError, "");
            }
        }
    }
}