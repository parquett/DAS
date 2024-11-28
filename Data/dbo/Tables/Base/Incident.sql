CREATE TABLE [dbo].[Incident]
(
	[IncidentId] BIGINT NOT NULL PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(MAX) NULL, 
    [DeletedBy] BIGINT NULL, 
    [CreatedBy] BIGINT NOT NULL, 
    [DateCreated] DATETIME NULL, 
    [DateUpdated] DATETIME NULL

)
