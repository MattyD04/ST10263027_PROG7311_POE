CREATE TABLE [dbo].[Farmers]
(
	[FarmerId] INT NOT NULL PRIMARY KEY, 
    [farmerUserName] NVARCHAR(MAX) NULL, 
    [farmerPassword] NVARCHAR(MAX) NULL, 
    [farmerContactNum] NVARCHAR(MAX) NULL
)
