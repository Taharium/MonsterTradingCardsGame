﻿using System.Globalization;

namespace MonsterTradingCardsGame.DTOs;

public class TradingHistoryDetailedDTO {
    public string Id { get; set; } = "";
    public string Offerer { get; set; } = "";
    public string CardToTrade { get; set; } = "";
    public string OffererCardName { get; set; } = "";
    public string OffererCardType { get; set; } = "";
    public float OffererCardDamage { get; set; }
    public string Type { get; set; } = "";
    public float MinimumDamage { get; set; }
    public string Trader { get; set; } = "";
    public string CardToReceive { get; set; } = "";
    public string TraderCardName { get; set; } = "";
    public string TraderCardType { get; set; } = "";
    public float TraderCardDamage { get; set; }

    public override string ToString() {
        return $"TradingId: {Id}; Type: {Type}; MinimumDamage: {MinimumDamage.ToString(CultureInfo.InvariantCulture)};\n\t\tOfferer: {Offerer}; CardToTrade: {CardToTrade} => \n\t\t(OffererCardName: {OffererCardName}, OffererCardType: {OffererCardType}, OffererCardDamage: {OffererCardDamage.ToString(CultureInfo.InvariantCulture)});\n\t\tTrader: {Trader}; CardToReceive: {CardToReceive} => \n\t\t(TraderCardName: {TraderCardName}, TraderCardType: {TraderCardType}, TraderCardDamage: {TraderCardDamage.ToString(CultureInfo.InvariantCulture)}),";
    }
}