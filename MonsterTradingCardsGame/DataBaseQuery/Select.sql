
SELECT * FROM users;
SELECT * FROM auth_tokens
JOIN public.users u on u.u_id = auth_tokens.fk_u_id;

SELECT max(auth_tokens.fk_u_id) FROM auth_tokens;



SELECT MIN(packageid) AS min FROM package WHERE fk_u_id IS NULL;

SELECT cardid, damage, name FROM deck JOIN public.cards c on deck.cardid = c.id WHERE fk_u_id = (SELECT u_id FROM users WHERE username = 'hello2');

SELECT * From cards JOIN public.stack s on cards.id = s.cardid WHERE id = '1cb6ab86-bdb2-47e5-b6e4-68c5ab389334';
SELECT * From cards JOIN public.stack d on cards.id = d.cardid WHERE id = '951e886a-0fbf-425d-8df5-af2ee4830d85';
SELECT * FROM tradingoffer WHERE fk_u_id = (SELECT u_id FROM users WHERE username = 'kienboec');
SELECT * FROM trading 
    JOIN public.tradingoffer t on t.tradeid = trading.fk_to_id 
    JOIN public.users u2 on u2.u_id = trading.fk_u_id 
    JOIN public.users u on u.u_id = t.fk_u_id 
    JOIN public.cards c on c.id = trading.cardid 
    JOIN public.cards c2 on c2.id = t.cardid 
         WHERE (t.fk_u_id = (SELECT u_id FROM users WHERE username = 'kienboec') OR trading.fk_u_id = (SELECT u_id FROM users WHERE username = 'kienboec') )AND traded = True;


SELECT u.username, u2.username, status1, status2, battlelog FROM battle JOIN public.users u on u.u_id = battle.enemyid JOIN public.users u2 on u2.u_id = battle.playerid WHERE (u.username = 'kienboec' OR u2.username = 'kienboec');
-- TODO: get card details --> join with cards table two times