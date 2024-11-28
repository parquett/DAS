CREATE TABLE [dbo].[Token]
(
	[TokenId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [RefreshToken] NVarCHAR(250) NOT NULL, 
    [DateExpires] DATETIME NULL ,
    [UserId] BIGINT NOT NULL,
    [DeviceGuidId] uniqueidentifier NULL ,
    [DeletedBy]   BIGINT         NULL,
    [CreatedBy]   BIGINT         NULL,
    [DateCreated] DATETIME       NULL,
    [DateUpdated] DATETIME       NULL
)
