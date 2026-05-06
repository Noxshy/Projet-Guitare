CREATE TABLE [dbo].[Bois]
(
    [IdBois] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [NomBois] VARCHAR(50),
    [TypeBois] VARCHAR(20),
    [Quantite] INT,
    [Disponible] BIT,
    [IdCouleur] INT NOT NULL,
    FOREIGN KEY ([IdCouleur]) REFERENCES [dbo].[Couleur]([IdCouleur])
);
