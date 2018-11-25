CREATE TABLE [dbo].[Tours]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TourStartTime] DATETIME NOT NULL, 
    [TourEndTime] DATETIME NOT NULL, 
    [Name] TEXT NOT NULL, 
    [Description] TEXT NOT NULL, 
    [Area] FLOAT NOT NULL, 
    [Location] TEXT NOT NULL
)
