CREATE TABLE [dbo].[SuiviFabrication]
(
    [IdSuivi] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [DatePhoto] DATETIME2,
    [UrlPhoto] VARCHAR(255),
    [Commentaire] VARCHAR(255),
    [IdCommande] INT NOT NULL,
    FOREIGN KEY ([IdCommande]) REFERENCES [dbo].[Commande]([IdCommande])
);
