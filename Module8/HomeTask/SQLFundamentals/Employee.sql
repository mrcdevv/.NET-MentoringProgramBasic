CREATE TABLE [dbo].[Employee]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [AddressId] INT NOT NULL, 
    [PersonId] INT NOT NULL, 
    [CompanyName] NVARCHAR(20) NOT NULL, 
    [Position] NVARCHAR(30) NULL, 
    [EmployeeName] NVARCHAR(100) NULL,

    CONSTRAINT FK_Employee_Address FOREIGN KEY ([AddressId]) 
        REFERENCES [dbo].[Address]([Id])
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT FK_Employee_Person FOREIGN KEY ([PersonId]) 
        REFERENCES [dbo].[Person]([Id])
        ON DELETE CASCADE
        ON UPDATE CASCADE
)
