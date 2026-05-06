CREATE TABLE [dbo].[Config_Micro]
(
    [IdMicro] INT NOT NULL,
    [IdConfiguration] INT NOT NULL,
    [Position_] INT,
    PRIMARY KEY ([IdMicro], [IdConfiguration]),
    FOREIGN KEY ([IdMicro]) REFERENCES [dbo].[Micro]([IdMicro]),
    FOREIGN KEY ([IdConfiguration]) REFERENCES [dbo].[Configuration]([IdConfiguration])
);
