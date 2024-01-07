start transaction;

UPDATE stack SET fk_u_id = (SELECT u_id FROM users WHERE username = 'hello2') WHERE cardid = 'd7d0cb94-2cbf-4f97-8ccf-9933dc5354b8' AND fk_u_id = (SELECT u_id FROM users WHERE username = 'hello3');

SELECT * FROM stack WHERE fk_u_id = (SELECT u_id FROM users WHERE username = 'hello3');

rollback;