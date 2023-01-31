-- Active: 1673975047108@@SG-TestDB-7110-mysql-master.servers.mongodirector.com@3306@Keepr

CREATE TABLE
    IF NOT EXISTS accounts(
        id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
        createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
        updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
        name varchar(255) COMMENT 'User Name',
        email varchar(255) COMMENT 'User Email',
        picture varchar(255) COMMENT 'User Picture',
        coverImg VARCHAR(255) COMMENT 'User Cover Image'
    ) default charset utf8 COMMENT '';

CREATE TABLE
    if not exists keeps(
        id int not null PRIMARY KEY AUTO_INCREMENT,
        createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
        updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
        creatorId varchar(255) COMMENT 'Creator Id',
        name VARCHAR(255) not null COMMENT 'Keep Name',
        description TEXT not null,
        img TEXT not null,
        views int not null default 0 COMMENT 'Number of Views',
        FOREIGN KEY (creatorId) REFERENCES accounts(id)
    ) DEFAULT charset utf8 COMMENT '';

CREATE TABLE
    if not exists vaults(
        id int not null PRIMARY KEY AUTO_INCREMENT,
        createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
        updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
        creatorId varchar(255) COMMENT 'Creator Id',
        name VARCHAR(255) not null COMMENT 'Vault Name',
        description TEXT not null,
        img TEXT not null,
        isPrivate BOOLEAN not null default false COMMENT 'Is Private',
        FOREIGN KEY (creatorId) REFERENCES accounts(id)
    ) DEFAULT charset utf8 COMMENT '';

CREATE TABLE
    if not exists vaultkeeps(
        id int not null PRIMARY KEY AUTO_INCREMENT,
        createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
        updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
        creatorId varchar(255) COMMENT 'Creator Id',
        vaultId int not null COMMENT 'Vault Id',
        keepId int not null COMMENT 'Keep Id',
        FOREIGN KEY (vaultId) REFERENCES vaults(id) ON DELETE CASCADE,
        Foreign Key (creatorId) REFERENCES accounts(id) ON DELETE CASCADE,
        FOREIGN KEY (keepId) REFERENCES keeps(id) ON DELETE CASCADE
    ) DEFAULT charset utf8 COMMENT '';

drop table vaultkeeps;