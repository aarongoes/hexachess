---
title: 'Hexachess Databasedocument'
description: 'In dit document wordt de implementatie van de verschillende SQL-leerdoelen van dit semester gedocumenteerd.'
author: 'Aaron Goes'
publishedAt: '14-06-2019'
---

# Hexachess SQL Leerdoelen

| Klas  |        S23 |
| :---- | ---------: |
| Datum | 14-06-2019 |

## Inhoud

[Inleiding](#inleiding)  
[Stored procedures](#stored-procedures-gebruiken-voor-het-ophalen-van-data)  
[Triggers](#triggers-maken-voor-het-afvangen-van-database-constraints-of-voor-het-bijwerken-van-gerelateerde-entiteiten)  
[INNER JOIN](#toepassingen-tonen-voor-zowel-de-inner-join-als-de-outer-join)  
[GROUP BY](#groepsfuncties-op-een-zinnige-manier-gebruiken-in-combinatie-met-een-group-by-clausule)

## Inleiding

In dit document wordt de implementatie van de verschillende SQL-leerdoelen van dit semester gedocumenteerd.

## Stored procedures gebruiken voor het ophalen van data

### GameUserNameAndGameCount

```sql
CREATE DEFINER=`u1005p1492_aaron`@`%` PROCEDURE `GameUserNameAndGameCount`(IN param1 INT)
BEGIN
  SELECT `name`, COUNT(game.id)
  FROM `user`
  INNER JOIN game
  ON `user`.id = game.firstplayerid
  WHERE `user`.id = param1;
END;
```

De functie van deze stored procedure is om het totaal aantal games op te halen die de geselecteerde user heeft gestart, om weer te geven bij de gebruikersinformatie van de speler.

### GetGameModesAmount

```sql
CREATE DEFINER=`u1005p1492_aaron`@`%` PROCEDURE `GetGameModesAmount`(IN param1 INT)
BEGIN
  SELECT `mode`, COUNT(id)
  FROM game
  WHERE firstplayerid = param1
  GROUP BY `mode`;
END;
```

De functie van deze stored procedure is om de hoeveelheid verschillende gamemode types waar de geselecteerde gebruiker aan heeft deelgenomen op te halen om weer te geven bij de gebruikersinformatie van de speler.

## Triggers maken voor het afvangen van database constraints of voor het bijwerken van gerelateerde entiteiten

### SetStartTime

```sql
CREATE DEFINER=`u1005p1492_aaron`@`%`
TRIGGER `setStartTime`
BEFORE INSERT ON `Untitled`
FOR EACH ROW
BEGIN
  SET new.datestart = CURRENT_TIMESTAMP;
END;
```

Deze trigger stelt de starttijd in van een game wanneer deze wordt aangemaakt in de database.

### SetDefaultThumbnail

```sql
CREATE DEFINER=`u1005p1492_aaron`@`%`
TRIGGER `setDefaultThumbnail`
BEFORE INSERT ON `Untitled`
FOR EACH ROW
BEGIN
  SET new.thumbnail = FROM_BASE64('CiAgICAgICA...');
END;

```

Deze trigger stelt de standaard thumbnail in van een game wanneer deze wordt aangemaakt in de database.

## Toepassingen tonen voor de INNER JOIN.

### GameUserNameAndGameCount

```sql
SELECT `name`, COUNT(game.id)
FROM `user`
INNER JOIN game
ON `user`.id = game.firstplayerid
WHERE `user`.id = param1;

```

Dit is de enige zinnige toepassing die ik kon bedenken voor een join functie binnen mijn applicatie, aangezien ik geen tabellen heb die een één op één relatie met elkaar hebben. Deze query zal precies hetzelfde resultaat geven wanneer er een left join in plaats van inner join zou worden gebruikt, omdat op het moment dat er geen game.id’s gekoppeld zijn aan de user de count van het game.id 0 zal zijn. De functie van deze query is om het totaal aantal games op te halen die de geselecteerde user heeft gestart, om weer te geven bij de gebruikersinformatie van de speler.

## Groepsfuncties op een zinnige manier gebruiken in combinatie met een GROUP BY-clausule.

### GetGameModesAmount

```sql
SELECT `mode`, COUNT(id)
FROM game
WHERE firstplayerid = param1 OR secondplayerid = param1
GROUP BY `mode`;
```

Deze group by query selecteert de hoeveelheid verschillende gamemode types waar de geselecteerde gebruiker aan heeft deelgenomen om weer te geven bij de gebruikersinformatie van de speler.