/*
Plantilla de script posterior a la implementación							
--------------------------------------------------------------------------------------
 Este archivo contiene instrucciones de SQL que se anexarán al script de compilación.		
 Use la sintaxis de SQLCMD para incluir un archivo en el script posterior a la implementación.			
 Ejemplo:      :r .\miArchivo.sql								
 Use la sintaxis de SQLCMD para hacer referencia a una variable en el script posterior a la implementación.		
 Ejemplo:      :setvar TableName miTabla							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

INSERT INTO [dbo].[Person] (Id, FirstName, LastName) VALUES 
(1, 'Marco', 'Morales'),
(2, 'Jane', 'Smith');

INSERT INTO [dbo].[Address] (Id, Street, City, State, ZipCode) VALUES 
(1, '123 Main St', 'New York', 'NY', '10001'),
(2, '456 Elm St', 'Los Angeles', 'CA', '90001');

INSERT INTO [dbo].[Employee] (Id, AddressId, PersonId, CompanyName, Position, EmployeeName) VALUES 
(1, 1, 1, 'EPAM', 'Developer', 'Marco Morales');

INSERT INTO [dbo].[Company] (Id, Name, AddressId) VALUES 
(1, 'EPAM', 1);
