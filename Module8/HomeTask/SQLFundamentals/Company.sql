CREATE TABLE [dbo].[Company]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(20) NOT NULL, 
    [AddressId] INT NOT NULL

    CONSTRAINT FK_Company_Address FOREIGN KEY ([AddressId]) 
        REFERENCES [dbo].[Address]([Id])
        ON DELETE CASCADE
        ON UPDATE CASCADE
)
