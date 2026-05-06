CREATE TABLE [dbo].[Utilisateur]
(
    [IdUtilisateur] INT IDENTITY(1,1) PRIMARY KEY,
    [Nom] VARCHAR(50),
    [Prenom] VARCHAR(50),
    [Email] VARCHAR(100) NOT NULL,
    [MotDePasse] VARBINARY(64) NOT NULL,
    [Sel] VARBINARY(64) NOT NULL,
    [IdRoleUtilisateur] INT NOT NULL,
    CONSTRAINT UQ_Client_Email UNIQUE (Email),
    CONSTRAINT FK_Client_RoleUtilisateur
        FOREIGN KEY (IdRoleUtilisateur)
        REFERENCES [dbo].[RoleUtilisateur](IdRoleUtilisateur)
);
