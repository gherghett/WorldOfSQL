
```sql

-- create
CREATE TABLE Player (
  id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  name VARCHAR(64) NOT NULL,
  email VARCHAR(64) NOT NULL,
  password VARCHAR(20)
);

-- insert
INSERT INTO Player (name, email, password) VALUES ( 'Clark', 'clark@Sales.se', 'pass');

-- fetch 
SELECT * FROM Player WHERE id = 1;

CREATE TABLE Room (
  Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  Name VARCHAR(64) NOT NULL
);

INSERT INTO Room (Name) VALUES ("First Big ol room");
INSERT INTO Room (Name) VALUES ("NEXT Room 2");
INSERT INTO Room (Name) VALUES ("The mysterious room");

SELECT * FROM Room;

CREATE TABLE Door (
  Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  Description TEXT,
  FromRoomId INT NOT NULL,
  ToRoomId INT NOT NULL,
  Locked BOOLEAN NOT NULL DEFAULT FALSE,
  FOREIGN KEY (FromRoomId) REFERENCES Room(Id),
  FOREIGN KEY (ToRoomId) REFERENCES Room(Id)
);

-- a door leading ONE WAY
INSERT INTO Door (Description, FromRoomId, ToRoomId) VALUES ('A door.', 1, 2);

-- heres a door to another room, with a way back.
INSERT INTO Door SET Description='A mysterious door', FromRoomId=1, ToRoomId =3, Locked=TRUE;
INSERT INTO Door SET Description='Door to big ol room', FromRoomId=3, ToRoomId=1;

-- Suppose we are in Room 1, this are the doors leading out.
SELECT * FROM Door WHERE FromRoomId = 1;

-- No way out, will not return anything
SELECT * FROM Door WHERE FromRoomId = 2;

CREATE TABLE ItemTypes (
  Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  Name VARCHAR(64) NOT NULL UNIQUE
);

-- The Items BASE class
CREATE TABLE Item (
  Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT, -- other item tables will SHARE the span of this key 
  Name VARCHAR(64) NOT NULL,
  ItemTypeId INT NOT NULL,
  FOREIGN KEY (ItemTypeId) REFERENCES ItemTypes(Id)
);

INSERT INTO ItemTypes (Name) VALUES ('Key'), ('Weapon');

CREATE TABLE PlayerItem (
  Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  PlayerId INT NOT NULL,
  ItemId INT NOT NULL,
  FOREIGN KEY (PlayerId) REFERENCES Player(Id),
  FOREIGN KEY (ItemId) REFERENCES Item(Id)
);

-- Key is needed if door is locked
CREATE TABLE KeyItem(
  Id INT PRIMARY KEY NOT NULL,
  Description TEXT,
  DoorId INT, -- door it unlocks
  FOREIGN KEY (DoorId) REFERENCES Door(Id),
  FOREIGN KEY (Id) REFERENCES Item(Id) -- This means that this eill basically be in the Item table
);

-- To make a key item we first insert the BASE table row
-- Insert into Items table
INSERT INTO Item (Name, ItemTypeId) 
VALUES ('Mysterious key', 1); -- Assuming 2 is the ItemTypeId for 'KeyItem'
-- Get the last inserted Item Id
SET @ItemId = LAST_INSERT_ID();
-- The mysterious door is locked lets make a KeyItem
INSERT INTO KeyItem (Id, Description, DoorId) VALUES (@ItemId, 'Mysterious key', 3);
-- these two rows in diffrent table now share the same primary key

-- This table has the (inital) location of a item ie key, if it is just laying in a Room
-- later perhaps there will be other tables for other places there can be items
CREATE TABLE ItemInRoom(
  Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  Description TEXT,
  RoomId INT,
  ItemId INT,
  FOREIGN KEY (RoomId) REFERENCES Room(Id),
  FOREIGN KEY (ItemId) REFERENCES Item(Id)
);


-- And place the key in the strating Room
INSERT INTO ItemInRoom (Description, RoomId, ItemId)  
VALUES ('Something on the floor...', 1, 1);

-- Like this you can find all items laying in a room
SELECT i.Name, k.Description 
FROM Item i
JOIN KeyItem k on i.Id = k.Id
JOIN ItemInRoom on  i.Id = ItemInRoom.ItemId
WHERE ItemInRoom.RoomId = 1;

-- this gives the item 1 to player one
INSERT INTO PlayerItem (PlayerId, ItemId) VALUES (1, 1);

-- Like this you can find all items a player has
SELECT i.Name, k.Description 
FROM Item i
JOIN KeyItem k on i.Id = k.Id
JOIN PlayerItem pi on  i.Id = pi.ItemId
WHERE pi.PlayerId = 1;
```
