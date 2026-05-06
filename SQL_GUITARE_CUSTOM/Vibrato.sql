CREATE TABLE [dbo].[Vibrato]
(
    [IdVibrato] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [NomModel] VARCHAR(50),
    [Marque] VARCHAR(50),
    [Quantite] INT,
    [Disponible] BIT
);
