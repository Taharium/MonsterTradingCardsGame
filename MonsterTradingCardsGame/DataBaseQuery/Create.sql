
CREATE TABLE users (
    u_id        SERIAL PRIMARY KEY,
    username    varchar(40) NOT NULL UNIQUE,
    password	varchar(40) NOT NULL,
    coins   	integer NOT NULL DEFAULT 20,
    name        varchar(40) NOT NULL DEFAULT '',
	bio         varchar(128)NOT NULL DEFAULT '',
	image		varchar(10) NOT NULL DEFAULT '',
	elo			integer NOT NULL DEFAULT 100,
	wins		integer NOT NULL DEFAULT 0,
	losses		integer NOT NULL DEFAULT 0,
	CHECK(coins >= 0)
);

CREATE TABLE auth_tokens (
    t_id        SERIAL PRIMARY KEY,
    token       varchar(60) NOT NULL UNIQUE,
    fk_u_id     integer NOT NULL,
    FOREIGN KEY (fk_u_id) REFERENCES users(u_id) ON DELETE CASCADE
);

CREATE TABLE cards (
    c_id        SERIAL PRIMARY KEY,
    id		    varchar(40) NOT NULL UNIQUE,
    name        varchar(40) NOT NULL,
    damage      real NOT NULL,
    element     varchar(40) NOT NULL,
    cardtype    varchar(40) NOT NULL,
    species   	varchar(40) NOT NULL
);

CREATE TABLE stack (
	s_id        SERIAL PRIMARY KEY,
    cardId      varchar(40) NOT NULL UNIQUE,
	fk_u_id     integer NOT NULL,
    FOREIGN KEY (cardId) REFERENCES cards(id) ON DELETE CASCADE,
	FOREIGN KEY (fk_u_id) REFERENCES users(u_id) ON DELETE CASCADE
);

CREATE TABLE deck (
	d_id		SERIAL PRIMARY KEY,
    cardId      varchar(40) NOT NULL UNIQUE,
    fk_u_id     integer NOT NULL,
    FOREIGN KEY (cardId) REFERENCES cards(id) ON DELETE CASCADE,
    FOREIGN KEY (fk_u_id) REFERENCES users(u_id) ON DELETE CASCADE
);

CREATE TABLE package (
    cardId      varchar(40) PRIMARY KEY,
    packageId   integer NOT NULL,
    fk_u_id     integer NULL,
    FOREIGN KEY (cardId) REFERENCES cards(id) ON DELETE CASCADE,
    FOREIGN KEY (fk_u_id) REFERENCES users(u_id) ON DELETE CASCADE
);

CREATE TABLE tradingoffer (
    to_id       SERIAL PRIMARY KEY,
    tradeId     varchar(40) NOT NULL UNIQUE,
    minDamage   real NOT NULL,
    type        varchar(40) NOT NULL,
    cardId      varchar(40) NOT NULL,
    traded      boolean NOT NULL DEFAULT false,
    fk_u_id     integer NOT NULL,
    FOREIGN KEY (cardId) REFERENCES cards(id) ON DELETE CASCADE,
    FOREIGN KEY (fk_u_id) REFERENCES users(u_id) ON DELETE CASCADE
);

CREATE TABLE trading (
    t_id        SERIAL PRIMARY KEY,
    cardId      varchar(40) NOT NULL,
    fk_u_id     integer NOT NULL,
    fk_to_Id    varchar(40) NOT NULL,
    FOREIGN KEY (cardId) REFERENCES cards(id) ON DELETE CASCADE,
    FOREIGN KEY (fk_to_Id) REFERENCES tradingoffer(tradeId) ON DELETE CASCADE,
    FOREIGN KEY (fk_u_id) REFERENCES users(u_id) ON DELETE CASCADE
);

CREATE TABLE battle (
    b_id        SERIAL PRIMARY KEY,
    playerid    integer NOT NULL,
    enemyid     integer NOT NULL,
    status1     bool NOT NULL,
    battlelog   text NOT NULL,
    status2     bool NOT NULL,
    FOREIGN KEY (enemyid) REFERENCES users(u_id) ON DELETE CASCADE,
    FOREIGN KEY (playerid) REFERENCES users(u_id) ON DELETE CASCADE
);