CREATE TABLE [dbo].[Commande]
(
    [IdCommande] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [DateCommande] DATETIME2,
    [StatutCommande] VARCHAR(20),
    [IdConfiguration] INT NOT NULL,
    [IdUtilisateur] INT NOT NULL,
    UNIQUE ([IdConfiguration]),
    FOREIGN KEY ([IdConfiguration]) REFERENCES [dbo].[Configuration]([IdConfiguration]),
    FOREIGN KEY ([IdUtilisateur]) REFERENCES [dbo].[Utilisateur]([IdUtilisateur])
);
