INSERT INTO users (username, password, coins, name, bio, image, elo, wins, losses)
VALUES ('admin', 'admin', 1000, 'Admin', 'I am the admin', 'admin', 1000, 0, 0);

INSERT INTO auth_tokens (token, fk_u_id)
VALUES ('admin-mtcgToken', 1);


INSERT INTO users (username, password, coins, name, bio, image, elo, wins, losses)
VALUES ('hello3', '56789', 20, 'Hello3', 'I am the admin2', ':-)', 100, 0, 0);

-- Insert into cards
/*INSERT INTO cards (id, name, damage, element, cardtype, species)
VALUES ('1id', 'WaterGoblin', 10.0, 'Water', 'Monster', 'Goblin');
INSERT INTO cards (id, name, damage, element, cardtype, species)
VALUES ('2id', 'FireGoblin', 10.0, 'Fire', 'Monster', 'Goblin');
INSERT INTO cards (id, name, damage, element, cardtype, species)
VALUES ('3id', 'RegularGoblin', 10.0, 'Regular', 'Monster', 'Goblin');
INSERT INTO cards (id, name, damage, element, cardtype, species)
VALUES ('4id', 'WaterTroll', 10.0, 'Water', 'Monster', 'Troll');
INSERT INTO cards (id, name, damage, element, cardtype, species)
VALUES ('5id', 'FireSpell', 10.0, 'Fire', 'Spell', 'Spell');
*/
-- change with just id
-- Insert into package
/*INSERT INTO PACKAGE (cardList, fk_u_id)
VALUES
    (
        ARRAY(
                SELECT
                    ROW(c_id, id, name, damage, element, cardtype, species)::cards
                FROM
                    cards
                WHERE
                    id IN ('1id', '2id', '3id', '4id', '5id')
        ),
        1
    );*/
-- insert into package
/*SELECT * From users;

INSERT INTO package (cardId, packageId) VALUES  ( '1id', 1 );
INSERT INTO package (cardId, packageId) VALUES  ( '2id', 1 );
INSERT INTO package (cardId, packageId) VALUES  ( '3id', 1 );
INSERT INTO package (cardId, packageId) VALUES  ( '4id', 1 );
INSERT INTO package (cardId, packageId) VALUES  ( '5id', 1 );*/

-- insert into package
--INSERT INTO package (cardId, packageId) VALUES  ( '1id', 2 );

-- get highest number of packageId
-- INSERT INTO package (cardId, packageId) VALUES  ( @cardId, @packageId) RETURNING max(packageId);
-- get highest number of packageId
/*SELECT MAX(packageId) FROM package;  -- use lock for thread safety

SELECT * From package 
JOIN cards c on c.id = package.cardId;

DELETE FROM cards WHERE id = '1id';*/




-- select every card as a cards from package
-- SELECT * FROM try, LATERAL unnest(cards) AS cards;

-- dictionary<string, string>

-- Insert into stack
/*INSERT INTO stack (cards, fk_u_id) VALUES
    (
                (SELECT
                    ROW(c_id, id, name, damage, element, cardtype, species)::cards
                FROM
                    cards
                WHERE
                    id = 
                )
        ,
        1
    );
*/