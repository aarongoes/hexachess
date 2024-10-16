-- Create `game` table if it doesn't exist
CREATE TABLE IF NOT EXISTS game
(
    id INT AUTO_INCREMENT PRIMARY KEY,
    firstplayerid INT NULL,
    token VARCHAR(255) NULL,
    dateend DATETIME NULL,
    datestart DATETIME DEFAULT CURRENT_TIMESTAMP NULL,
    mode INT NOT NULL,
    thumbnail LONGBLOB NULL,
    winner INT NULL,
    secondplayerid INT NULL,
    gameid INT NULL
)
COLLATE = utf8mb4_unicode_ci;

-- Trigger to update `dateend` when game record is updated
DELIMITER $$
CREATE DEFINER = root@`%` TRIGGER before_game_update
BEFORE UPDATE ON game
FOR EACH ROW
BEGIN
    SET NEW.dateend = CURRENT_TIMESTAMP;
END$$
DELIMITER ;

-- Create `move` table if it doesn't exist
CREATE TABLE IF NOT EXISTS move
(
    id INT AUTO_INCREMENT PRIMARY KEY,
    newx DECIMAL(5,2) NOT NULL,
    newy DECIMAL(5,2) NOT NULL,
    oldx DECIMAL(5,2) NOT NULL,
    oldy DECIMAL(5,2) NOT NULL,
    gameid INT NULL,
    moveid INT NULL,
    player INT NULL
);

-- Create `user` table if it doesn't exist
CREATE TABLE IF NOT EXISTS user
(
    id INT AUTO_INCREMENT PRIMARY KEY,
    token VARCHAR(255) NULL,
    name VARCHAR(255) NULL,
    password VARCHAR(255) NULL
);

-- Procedure to get username and game count
DELIMITER $$
CREATE DEFINER = root@`%` PROCEDURE GameUserNameAndGameCount(IN userId INT)
BEGIN
    SELECT u.name AS userName,
           (SELECT COUNT(*)
            FROM game g
            WHERE g.firstplayerid = userId OR g.secondplayerid = userId) AS gameCount
    FROM `user` u
    WHERE u.id = userId;
END$$
DELIMITER ;

-- Procedure to get game mode counts
DELIMITER $$
CREATE DEFINER = root@`%` PROCEDURE GetGameModesAmount(IN userId INT)
BEGIN
    SELECT g.mode AS gameMode,
           COUNT(g.id) AS modeCount
    FROM `game` g
    WHERE g.firstplayerid = userId OR g.secondplayerid = userId
    GROUP BY g.mode;
END$$
DELIMITER ;
