CREATE TABLE [dbo].[User] (
    [UserId]      BIGINT           IDENTITY (1, 1) NOT NULL,
    [Login]       NVARCHAR (100)   NOT NULL,
    [PasswordSalt]    NVARCHAR (150)     NOT NULL,
     [PasswordHash]    NVARCHAR (150)    NOT NULL,
    --[Timeout]     INT              NOT NULL,
    [Permission]  BIGINT         NULL,
	--[SID]         NVARCHAR (200)   NULsL,
    [RoleId]      BIGINT           NOT NULL,
    [PersonId]    BIGINT           NOT NULL,
    [UniqueId]    UNIQUEIDENTIFIER NULL,
    [LastLogin]   DATETIME         NULL,
    [ImageId]     BIGINT NULL,
    [Enabled]     BIT NOT NULL DEFAULT 1,
    [DeletedBy]   BIGINT           NULL,
    [CreatedBy]   BIGINT           NOT NULL,
    [DateCreated] DATETIME         NOT NULL
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_User_Person] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


