CREATE PROCEDURE [dbo].[InsertEmployee]
    @EmployeeName NVARCHAR(100) = NULL,
    @FirstName NVARCHAR(50) = NULL,
    @LastName NVARCHAR(50) = NULL,
    @CompanyName NVARCHAR(20),
    @Position NVARCHAR(30) = NULL,
    @Street NVARCHAR(50),
    @City NVARCHAR(20) = NULL,
    @State NVARCHAR(50) = NULL,
    @ZipCode NVARCHAR(50) = NULL
AS
BEGIN
    -- Validations
    IF (
        (@EmployeeName IS NULL OR LTRIM(RTRIM(@EmployeeName)) = '') AND
        (@FirstName IS NULL OR LTRIM(RTRIM(@FirstName)) = '') AND
        (@LastName IS NULL OR LTRIM(RTRIM(@LastName)) = '')
    )
    BEGIN
        RAISERROR ('At least one name field (EmployeeName, FirstName, or LastName) must be non-empty.', 16, 1);
        RETURN;
    END

    -- Truncate
    SET @CompanyName = LEFT(@CompanyName, 20);

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @AddressId INT;
        INSERT INTO [dbo].[Address] (Street, City, State, ZipCode)
        VALUES (@Street, @City, @State, @ZipCode);

        SET @AddressId = SCOPE_IDENTITY();

        INSERT INTO [dbo].[Employee] (EmployeeName, AddressId, PersonId, CompanyName, Position)
        VALUES (
            @EmployeeName,
            @AddressId,
            NULL,
            @CompanyName,
            @Position
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(), 
            @ErrorSeverity = ERROR_SEVERITY(), 
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;
