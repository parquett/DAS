CREATE TABLE [dbo].[IPAdress]
(
	[IPAdressId] BIGINT NOT NULL PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(MAX) NULL, 	
    [DeletedBy] BIGINT NULL, 
    [CreatedBy] BIGINT NOT NULL, 
    [DateCreated] DATETIME NOT NULL, 
    [DateUpdated] DATETIME NULL

)
