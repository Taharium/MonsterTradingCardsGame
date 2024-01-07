using System.Text.Json.Serialization;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Enums;
using MonsterTradingCardsGame.Extensions;

namespace MonsterTradingCardsGame.Models;

public class Card {
    public string Id { get; set; } = "";

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CardName Name { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CardElementType ElementType { get; private set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CardSpecies Species { get; private set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CardType Type { get; private set; }

    public float Damage { get; set; }

    public Card(string id, string name, float damage) {
        Id = id;
        Damage = damage;
        SetProperties(name);
    }

    public Card() {}
        
    public Card(CardDTO c) {
        Id = c.Id;
        Damage = c.Damage;
        SetProperties(c.Name);
    }

    public void SetProperties(string name) {
        Name = name.ToEnum();
        ElementType = EnumExtension.GetElementTypeFromName(Name);
        Species = EnumExtension.GetSpeciesFromName(Name);
        Type = EnumExtension.GetTypeFromName(Name);
    }
        
    public string ToCustomString() {
        return $"Id: {Id}, Name: {Name.ToString()}, Damage: {Damage}, Type: {Type.ToString()}, Species: {Species.ToString()}, Element: {ElementType.ToString()}";
    }
}