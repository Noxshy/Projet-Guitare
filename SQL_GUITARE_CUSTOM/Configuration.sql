CREATE TABLE [dbo].[Configuration]
(
    [IdConfiguration] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [NomConfiguration] VARCHAR(50),
    [DateCreation] DATETIME2,
    [DateModification] DATETIME2,
    [IdUtilisateur] INT NOT NULL,
    [IdVibrato] INT NOT NULL,
    FOREIGN KEY ([IdUtilisateur]) REFERENCES [dbo].[Utilisateur]([IdUtilisateur]),
    FOREIGN KEY ([IdVibrato]) REFERENCES [dbo].[Vibrato]([IdVibrato])
);
