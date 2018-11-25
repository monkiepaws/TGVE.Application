CREATE TABLE [dbo].[Clients]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] TEXT NOT NULL, 
    [LastName] TEXT NOT NULL, 
    [PhoneNumber] TEXT NOT NULL, 
    [Address] TEXT NOT NULL, 
    [DateOfBirth] DATE NOT NULL, 
    [Email] TEXT NOT NULL
)
