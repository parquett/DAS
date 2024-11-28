CREATE TABLE [dbo].[KeyValueSetting]
(
	[KeyValueSettingId] BIGINT NOT NULL PRIMARY KEY IDENTITY,
    [Key] NVARCHAR(50) NOT NULL, 
    [Value] NVARCHAR(500) NOT NULL,
    [CreatedBy]              BIGINT         NULL,
    [DeletedBy]              BIGINT         NULL, 
    [DateCreated]            DATETIME       NOT NULL
)
