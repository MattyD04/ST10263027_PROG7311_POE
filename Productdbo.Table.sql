CREATE TABLE [dbo].[Products]
(
	[ProductId] INT NOT NULL PRIMARY KEY, 
    [productName] NVARCHAR(MAX) NULL, 
    [productCategory] NVARCHAR(MAX) NULL, 
    [productionDate] DATETIME NULL,
    [FarmerId] INT NULL

	CONSTRAINT [FK_Products_Farmers] FOREIGN KEY ([FarmerId]) 
    REFERENCES [dbo].[Farmers] ([FarmerId]), 
    
)
