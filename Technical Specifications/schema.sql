/*
Server Structure & HTTP transactions with Unity:
Using a Unity Web Request (https://docs.unity3d.com/Manual/UnityWebRequest.html) we can send requests to a LAMP Stack Server (https://aws.amazon.com/what-is/lamp-stack/) and connect to a SQL database.

We should use a SQL database because we want rigid, secure, and relational data storage.

We can get server space with LAMP architecture on Digital Ocean for relatively cheap.
*/

/*
Password Hashing Path:
plaintext pw on logon,
hash n times -> store in local data,
hash n times -> send to server,
salt on server -> store in db
*/

CREATE TABLE users (
    id int unsigned auto_increment primary key,
    username varchar(64) not null,
    password char(64) not null,
    created timestamp default current_timestamp,
    last_used timestamp default current_timestamp on update current_timestamp
);

CREATE TABLE inventory (
    id int unsigned primary key, /* users -> id */
    /* whatever data goes into a users inventory collection */
);

CREATE TYPE department_enum AS ENUM (
    "Math",
    "English",
    "Science",
    "History",
    "Language",
    "Arts",
    "Administration",
    "Maintenince",
    "Other"
);

CREATE TYPE rarity_enum AS ENUM (
    "Common",
    "Uncommon",
    "Rare",
    "Epic",
    "Legendary",
    "Mythic"
);

CREATE TABLE faculty (
    id int unsigned auto_increment primary key,
    name varchar(64) not null,
    item varchar(64) not null,
    pixelized_photo blob not null,
    pixelized_item blob not null,
    department department_enum,
    rarity rarity_enum,
    health double,
    damage double
);
