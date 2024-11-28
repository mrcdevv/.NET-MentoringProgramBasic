CREATE TRIGGER [dbo].[TRG_InsertEmployee_CreateCompany]
ON [dbo].[Employee]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AddressId INT;
    DECLARE @CompanyName NVARCHAR(20);

    SELECT 
        @AddressId = i.AddressId,
        @CompanyName = i.CompanyName
    FROM INSERTED i;

    IF NOT EXISTS (
        SELECT 1
        FROM [dbo].[Company]
        WHERE Name = @CompanyName
    )
    BEGIN
        DECLARE @NewAddressId INT;
        INSERT INTO [dbo].[Address] (Street, City, State, ZipCode)
        SELECT Street, City, State, ZipCode
        FROM [dbo].[Address]
        WHERE Id = @AddressId;

        SET @NewAddressId = SCOPE_IDENTITY();

        INSERT INTO [dbo].[Company] (Name, AddressId)
        VALUES (@CompanyName, @NewAddressId);
    END
END;
GO
