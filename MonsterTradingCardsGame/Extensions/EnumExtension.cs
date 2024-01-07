using System.Collections.Concurrent;
using MonsterTradingCardsGame.Enums;

namespace MonsterTradingCardsGame.Extensions;

public static class EnumExtension {
    private static readonly ConcurrentDictionary<CardName, CardElementType> CardElementToName = new ConcurrentDictionary<CardName, CardElementType>();
    private static readonly ConcurrentDictionary<CardName, CardSpecies> CardSpeciesToName = new ConcurrentDictionary<CardName, CardSpecies>();
    private static readonly ConcurrentDictionary<CardName, CardType> CardTypeToName = new ConcurrentDictionary<CardName, CardType>();
    
    public static void FillDictionaries() {
        foreach (CardName value in Enum.GetValues(typeof(CardName))) {
            CardSpeciesToName.TryAdd(value, GetEnumValueThroughString(value));
        }

        foreach (CardName value in Enum.GetValues(typeof(CardName))) {
            CardElementToName.TryAdd(value, SetElementType(value));
            if (value.ToString().EndsWith("Spell")) {
                CardTypeToName.TryAdd(value, CardType.Spell);
            } else {
                CardTypeToName.TryAdd(value, CardType.Monster);
            }
        }
    }

    public static CardName ToEnum(this string value) {
        return (CardName)Enum.Parse(typeof(CardName), value, true);
    }

    public static CardSpecies GetSpeciesFromName(CardName name) {
        return CardSpeciesToName[name];
    }

    public static CardElementType GetElementTypeFromName(CardName name) {
        return CardElementToName[name];
    }

    public static CardType GetTypeFromName(CardName name) {
        return CardTypeToName[name];
    }

    public static void ClearDictionaries() {
        CardElementToName.Clear();
        CardSpeciesToName.Clear();
        CardTypeToName.Clear();
    }

    public static CardElementType SetElementType(CardName name) {

        if (name.ToString().StartsWith(CardElementType.Water.ToString())) {
            return CardElementType.Water;
        }
        
        if (name.ToString().StartsWith(CardElementType.Fire.ToString())) {
            return CardElementType.Fire;
        }
        
        if (name.ToString().StartsWith(CardElementType.Regular.ToString())) {
            return CardElementType.Regular;
        }
        
        if (name.ToString().StartsWith(CardElementType.Ice.ToString())) {
            return CardElementType.Ice;
        }
        
        if (name.ToString().StartsWith(CardElementType.Ground.ToString())) {
            return CardElementType.Ground;
        }

        return CardElementType.Regular;
    }

    public static CardSpecies GetEnumValueThroughString(CardName name) {

        foreach (CardSpecies value in Enum.GetValues(typeof(CardSpecies))) {
            if (name.ToString().EndsWith(value.ToString())) {
                return value;
            }
        }
        return default;
    }

}