using MonsterTradingCardsGame.Enums;
using MonsterTradingCardsGame.Extensions;

namespace MonsterTradingCardsGame.Test;

internal class EnumExtensionTest {

    [SetUp]
    public void Setup() {
        EnumExtension.ClearDictionaries();
        EnumExtension.FillDictionaries();
    }

    [Test]
    public void TestSetElementType() {
        EnumExtension.SetElementType(CardName.FireElf);
        EnumExtension.SetElementType(CardName.WaterSpell);
        EnumExtension.SetElementType(CardName.RegularSpell);
        EnumExtension.SetElementType(CardName.Dragon);

        Assert.Multiple(() => {
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.FireElf), Is.EqualTo(CardElementType.Fire));
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.WaterSpell), Is.EqualTo(CardElementType.Water));
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.RegularSpell), Is.EqualTo(CardElementType.Regular));
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.Dragon), Is.EqualTo(CardElementType.Regular));
        });

    }

    [Test]
    public void TestGetEnumValueThroughString() {
        EnumExtension.GetEnumValueThroughString(CardName.Dragon);
        EnumExtension.GetEnumValueThroughString(CardName.FireElf);
        EnumExtension.GetEnumValueThroughString(CardName.WaterGoblin);
        EnumExtension.GetEnumValueThroughString(CardName.RegularSpell);

        Assert.Multiple(() => {
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.Dragon), Is.EqualTo(CardSpecies.Dragon));
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.FireElf), Is.EqualTo(CardSpecies.Elf));
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.WaterGoblin), Is.EqualTo(CardSpecies.Goblin));
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.RegularSpell), Is.EqualTo(CardSpecies.Spell));
        });

    }

    [Test]
    public void TestDictionarySpecies() {

        Assert.Multiple(() => {
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.Dragon), Is.EqualTo(CardSpecies.Dragon));
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.FireElf), Is.EqualTo(CardSpecies.Elf));
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.WaterGoblin), Is.EqualTo(CardSpecies.Goblin));
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.Knight), Is.EqualTo(CardSpecies.Knight));
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.Kraken), Is.EqualTo(CardSpecies.Kraken));
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.Ork), Is.EqualTo(CardSpecies.Ork));
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.Wizzard), Is.EqualTo(CardSpecies.Wizzard));
            Assert.That(EnumExtension.GetSpeciesFromName(CardName.RegularSpell), Is.EqualTo(CardSpecies.Spell));
        });
    }

    [Test]
    public void TestDictionaryElement() {

        Assert.Multiple(() => {
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.Dragon), Is.EqualTo(CardElementType.Regular));
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.FireElf), Is.EqualTo(CardElementType.Fire));
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.WaterGoblin), Is.EqualTo(CardElementType.Water));
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.Knight), Is.EqualTo(CardElementType.Regular));
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.Kraken), Is.EqualTo(CardElementType.Regular));
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.Ork), Is.EqualTo(CardElementType.Regular));
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.Wizzard), Is.EqualTo(CardElementType.Regular));
            Assert.That(EnumExtension.GetElementTypeFromName(CardName.RegularSpell), Is.EqualTo(CardElementType.Regular));
        });

    }

    [Test]
    public void TestDictionaryType() {

        Assert.Multiple(() => {
            Assert.That(EnumExtension.GetTypeFromName(CardName.Dragon), Is.EqualTo(CardType.Monster));
            Assert.That(EnumExtension.GetTypeFromName(CardName.FireElf), Is.EqualTo(CardType.Monster));
            Assert.That(EnumExtension.GetTypeFromName(CardName.WaterGoblin), Is.EqualTo(CardType.Monster));
            Assert.That(EnumExtension.GetTypeFromName(CardName.Knight), Is.EqualTo(CardType.Monster));
            Assert.That(EnumExtension.GetTypeFromName(CardName.Kraken), Is.EqualTo(CardType.Monster));
            Assert.That(EnumExtension.GetTypeFromName(CardName.Ork), Is.EqualTo(CardType.Monster));
            Assert.That(EnumExtension.GetTypeFromName(CardName.Wizzard), Is.EqualTo(CardType.Monster));
            Assert.That(EnumExtension.GetTypeFromName(CardName.FireSpell), Is.EqualTo(CardType.Spell));
            Assert.That(EnumExtension.GetTypeFromName(CardName.WaterSpell), Is.EqualTo(CardType.Spell));
            Assert.That(EnumExtension.GetTypeFromName(CardName.RegularSpell), Is.EqualTo(CardType.Spell));
        });
    }
}