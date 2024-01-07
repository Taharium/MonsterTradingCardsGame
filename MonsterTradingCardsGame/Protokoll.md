<div style="text-align: center;"><h1>Protokoll</h1></div>

## Design, Failure and lessons learned

### Anfang des Projekts (Angabe lesen)
Am Anfang habe ich nicht ganz verstanden wie das Projekt aufgebaut werden soll, dennoch war ich anfangs sehr motiviert.
Ich habe gedacht ich könnte voraus arbeiten und so schneller fertig werden. Doch erst später wurde mir bewusst, dass
alles mithilfe von HTTP Requests gemacht werden muss und nicht auf der Console mit Input. Ich hätte mir die 
Spezifikation bzw. den curl-script besser anschauen sollen, denn dann hätte ich bemerkt, dass der admin die Karten 
erstellt und nicht random durch eine Card-Factory wie es am anfang implementiert habe. Ich habe mich dann entschieden 
das Projekt neu zu starten. Die Models konnte ich noch beibehalten, da ich diese schon in der ersten Version gemacht 
habe.

### Server
Anfangs hatte ich Schwierigkeiten den Server zu gestalten und war sehr froh, dass unser Lektor uns dabei geholfen hat.
Im Server werden die Threads erstellt und die Anfragen werden behandelt. Der ClientProcessor wird instanziert Parsed 
die Anfrage und sucht dann im Dictionary nach dem ensprechenden Endpoint. Wenn der Endpoint gefunden wurde, dann wird
die ensprechende Klasse instanziert und die Anfrage wird behandelt. Gab es keine Fehler, dann wird die Response
zurückgegeben. Wenn es einen Fehler gab, dann wird eine ProcessException geworfen und die Fehlermeldung wird
zurückgegeben.

### Unit of Work Pattern
Gleich danach habe ich mit den Repositories und den dementsprechenden Handling angefangen. Mein Design hat mir jedoch
nicht gefallen. Glücklicherweise habe ich von Unit of Work Pattern gehört und habe mich sofort entschieden dies zu 
implementieren. Das hat mir sehr geholfen und ich konnte die Repositories und das Handling vereinfachen. Wenn eine 
Anfrage kommt, dann wird im Dictionary nach dem Endpoint gesucht und dann die ensprechende Klasse instanziert. In der 
Klasse wird dann die Anfrage behandelt, je nach dem was für eine Anfrage es ist (GET, POST, PUT, DELETE). Hier kommt 
auch das Unit of Work Pattern zum Einsatz. Die Repositories werden instanziert und die Anfrage wird behandelt.

### EnumExtension
Während des Projekts habe ich gemerkt, dass ich ab und zu mit stringliteralen arbeite, was ich nicht gut fand. Aus 
diesem Grund habe ich mich entschieden eine EnumExtension zu implementieren, um die stringliteralen zu vermeiden. 
Dadurch konnte ich meiner Meinung viel einfacher und übersichtlicher vergleiche erstellen bzw. herausfinden was für ein
Typ beispielsweise eine Karte ist. Es gibt insgesamt 3 Dictionarys, die die CardName als Key haben und die ensprechende
Enum (CardElementType, CardSpecies, CardType) als Value haben. Die werden am Anfang des Programms erstellt und befüllt
und während kann man ganz einfach durch die CardName herausfinden was für ein Typ die Karte ist, was für ein Element die
Karte hat und was für eine Species die Karte ist.

### ListExtension
Wenn ich eine Liste von Daten in PlainText zurückgebe, dann muss ich die Daten in eine Liste von Strings umwandeln.
Deshalb habe ich mich entschieden eine ListExtension zu implementieren, um die Daten in eine Liste von Strings
umzuwandeln. 

### JsonExtension
Die JsonExtension habe ich implementiert, da ich die Daten in Json umwandeln muss. Da die Daten jedoch nicht im Terminal
beautified werden, habe ich es durch die JsonExtension gemacht.

### DTO
Ich habe bemerkt, dass ich beim JsonSerialisieren der Models zu viel Information zurückgebe, die der Client nicht 
unbedingt braucht. Aus diesem Grind habe ich mich entschieden Data Transfer Objects zu implementieren. Die DTO's sind 
meiner Meinung nach sehr hilfreich gewesen und ich bin froh, dass ich diese implementiert habe. Jedes Mal wenn ich eine
spezielle Response brauchte, habe ich ein DTO erstellt. Dadurch konnte ich vermeiden die Models zu verändern.

### ProcessException
Ich habe mich entschieden eine ProcessException zu implementieren, da ich so die Fehlermeldungen besser handeln kann.
Ich habe ein Weg gesucht, um die Fehlermeldungen zu Customizen, weshalb ich meine eigene Exception erstellt habe. Das
war besser als mehrere verschiedene Exceptions zu verwenden und diese dann wieder zu catchen. Die ProcessException
werden in der jeweiligen Handling Klasse gefangen und in den Repositorys werden sie gethrowt. In einigen Ausnahmefällen
wird die ProcessException in der Handling Klasse gethrowt und dann wieder gefangen.

### Reflection
Ich habe ein zweites Projekt erstellt, um die Reflection zu testen. Ich habe mich für Reflection entschieden, da ich
dieses Thema sehr interessant finde und ich es gerne lernen wollte. Doch ich habe ziemlich schnell gemerkt, dass ich zu
wenig darüber weiß und habe mich entschieden es nicht zu implementieren. Ich habe erst am Ende erst ungefähr verstanden
wie es funktioniert und dafür war es zu spät.

## Unit Tests
Ich habe mich entschieden die Unit Tests zuerst zu machen, da ich so die Funktionalität der einzelnen Komponenten 
überprüfen konnte. Die Art und Weise wie ich Unit Tests angegangen bin, war zuerst die die eigentliche Komonente btw. 
Funktion oder Klasse zu schreiben und dann gleich danach die Unit Tests dazu zu schreiben. Ebenso habe ich bei Fehlern
Unit Tests geschrieben, um die Fehler zu finden bzw. zu beheben.

## Time spent
Ich möchte gleich am Anfang sagen, hätte ich die Angabe besser gelesen, hätte ich mir viel Zeit erspart. Dennoch möchte
ich darauf hinweisen, dass die Angabe bzw. die Spezifikation sehr unübersichtlich ist. Ich habe sehr viel Zeit damit
verbracht die Angabe zu lesen und zu verstehen. Sehr viel Zeit habe ich auch damit verbracht, den Server zu 
implementieren, da ich zuerst nicht wusste wie ich das machen soll und es mein erstes Mal war.
Am Anfang habe ich sehr viel Zeit damit verbracht, die Models zu erstellen und die Funktionalität zu implementieren, 
doch nach Beginn des Servers habe ich wieder nachgelassen. Mit Beginn der zweiten Präsentation habe ich wieder mehr
Zeit gehabt. Battle und Trade haben etwas gebraucht, weshalb ich sie in den Ferien gemacht habe. 

## Git link
https://github.com/Taharium/MonsterTradingCardsGame4.git

## Änderungen

### Request Body
Ich habe die 204 No Content Response zu Accepted 202 geändert, da ich ansonsten kein Response Body zurückgeben kann.

### Coins
Nach jedem Battle bekommt der Gewinner 2 Coins, um später weiter karten zu kaufen.

### Curl-Script
Ich habe das Curl-Script geändert. Dadurch, dass ich ein paar optianle Features implementiert habe, musste ich das
Curl-Script ergänzen um diese zu zeigen.

## Optional Features

### Zwei Elemente hinzugefügt
Ich habe zwei Elemente hinzugefügt, um das Spiel etwas interessanter zu machen. Ich habe mich für Eis und Boden
entschieden, da ich diese Elemente sehr interessant finde. Eis ist sehr effektiv gegen Boden, da Eis Boden einfrieren
kann. Boden ist sehr effektiv gegen Feuer, da Boden Feuer löschen kann. Feuer ist sehr effektiv gegen Eis, da Feuer Eis
schmelzen kann. Wasser ist sehr effektiv gegen Boden, da Wasser Boden aufweichen kann. Es gibt nun IceSpell, 
GroundSpell, IceGoblin, GroundGoblin, IceTroll, GroundTroll, IceElf und GroundElf.

### lose/win ratio
Ich habe eine lose/win ratio hinzugefügt, um zu sehen wie gut der User ist bzw. wie oft er gewonnen hat. Die lose/win 
ratio wird als float in Prozent zurückgegeben in Plaintext.

### Trade History
Ich habe eine TradeHistory hinzugefügt, um zu sehen welche Karten der User getradet hat. Die Response ist in Json
und man kann sehen welche cardid der User, sichtbar ist nur die UserId, getradet hat und welche er dafür bekommen hat.
Will man eine detailierte History dann fügt man einfach ?detailed=true hinzu. Dann kann man die Usernamen sehen 
und die details der Karten sowie die Requirements. Fügt man jedoch &format=plain hinzu, dann wird die Response in 
PlainText zurückgegeben.

#### Einfache Trading History
```
[
  {
    "Id": "6cd85277-4590-49d4-b0cf-ba0a921faad0",
    "Offerer": 1,
    "CardToTrade": "1cb6ab86-bdb2-47e5-b6e4-68c5ab389334",
    "Type": "monster",
    "MinimumDamage": 15,
    "Trader": 2,
    "CardToReceive": "951e886a-0fbf-425d-8df5-af2ee4830d85"
  }
]
```

#### detailierte Trading History

```
[
  {
    "Id": "6cd85277-4590-49d4-b0cf-ba0a921faad0",
    "Offerer": "kienboec",
    "CardToTrade": "1cb6ab86-bdb2-47e5-b6e4-68c5ab389334",
    "OffererCardName": "Ork",
    "OffererCardType": "Monster",
    "OffererCardDamage": 55,
    "Type": "monster",
    "MinimumDamage": 15,
    "Trader": "altenhof",
    "CardToReceive": "1cb6ab86-bdb2-47e5-b6e4-68c5ab389334",
    "TraderCardName": "Ork",
    "TraderCardType": "Monster",
    "TraderCardDamage": 45
  }
]
```

#### detailierte Trading History in Plaintext

```
[
        TradingId: 6cd85277-4590-49d4-b0cf-ba0a921faad0; Type: monster; MinimumDamage: 15;
                Offerer: kienboec; CardToTrade: 1cb6ab86-bdb2-47e5-b6e4-68c5ab389334 =>
                (OffererCardName: Ork, OffererCardType: Monster, OffererCardDamage: 55);
                Trader: altenhof; CardToReceive: 1cb6ab86-bdb2-47e5-b6e4-68c5ab389334 =>
                (TraderCardName: Ork, TraderCardType: Monster, TraderCardDamage: 45),
]
```

## Mandatory Unique Features

### Battle History
Ich habe mich entschieden eine BattleHistory als Unique Feature zu implementieren. Dadurch, dass ich ebenfalls eine
TradeHistory habe, habe ich eine HistoryHandling Klasse erstellt, die die beiden Historys verwaltet. Die BattleHistory
gibt eine Liste von Battle in der der User mitgespielt hat. Die Response wird als Json zurückgegeben. Man kann
sehen, ob der User gewonnen oder verloren hat und man kann die letzte zeile des BattleLog ebenfalls sehen. Dadurch kann 
der User sehen, in wie vielen Runden er gewonnen oder verloren hat.

#### Battle History
```
[
  {
    "Fight": "altenhof vs. kienboec",
    "Result": "altenhof won",
    "BattleLogShort": "Player1 (altenhof) wins the Game in 35 rounds: Player2 (kienboec) has no cards left"
  }
]
```
