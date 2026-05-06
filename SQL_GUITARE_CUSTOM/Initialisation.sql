IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'USER_BDD') BEGIN
    CREATE USER USER_BDD WITH PASSWORD = 'P455w0rd!';
    EXEC sp_addrolemember 'db_datareader', 'USER_BDD';
    EXEC sp_addrolemember 'db_datawriter', 'USER_BDD';
END;