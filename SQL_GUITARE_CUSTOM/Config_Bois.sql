CREATE TABLE [dbo].[Config_Bois]
(
    [IdBois] INT NOT NULL,
    [IdConfiguration] INT NOT NULL,
    [IdRoleBois] INT NOT NULL,
    PRIMARY KEY ([IdBois], [IdConfiguration], [IdRoleBois]),
    FOREIGN KEY ([IdBois]) REFERENCES [dbo].[Bois]([IdBois]),
    FOREIGN KEY ([IdConfiguration]) REFERENCES [dbo].[Configuration]([IdConfiguration]),
    FOREIGN KEY ([IdRoleBois]) REFERENCES [dbo].[RoleBois]([IdRoleBois])
);
