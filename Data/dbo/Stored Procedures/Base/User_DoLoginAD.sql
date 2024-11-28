--CREATE PROCEDURE [dbo].[User_DoLoginAD]
--(
--	@Login nvarchar(50),
--	@SID nvarchar(200),
--	@FirstName nvarchar(100),
--	@LastName nvarchar(100),
--	@Email nvarchar(50),
--	@Permission bigint,
--	@LaboratoryId bigint=null
--)

--AS
--	BEGIN
--		SET NOCOUNT ON;

--		DECLARE @existingSid nvarchar(200)
--		DECLARE @userId bigint
--		DECLARE @personId bigint

--		SELECT
--			@userId = u.UserId,
--			@existingSid = u.SID
--		FROM [User] u
--		WHERE u.[Login]=@Login and u.Enabled = 1 AND (u.SID IS NULL OR @SID = u.[SID])

--		IF @userId IS NULL
--		BEGIN
--			INSERT INTO [dbo].[Person]
--				   ([FirstName]
--				   ,[LastName]
--				   ,[Email]
--				   ,[SexId]
--				   ,[DeletedBy]
--				   ,[CreatedBy]
--				   ,[DateCreated])
--			 VALUES
--				   (@FirstName
--				   ,@LastName
--				   ,@Email
--				   ,1
--				   ,NULL
--				   ,1
--				   ,SYSDATETIME())

--			SET @personId = SCOPE_IDENTITY()

--			INSERT INTO BORevision([Table],BOId,BOName,[Date],UserId,Comment,Json,[Type])
--			VALUES('Person',@personId,'',dbo.GetZoneDate(),1,'AD insert person','',1)

--			IF(@LaboratoryId IS NOT NULL)
--			BEGIN
--				INSERT INTO PersonLaboratory(PersonId,LaboratoryId)
--				VALUES(@personId,@LaboratoryId)
--			END

--			INSERT INTO [User]
--				   ([Login]
--				   ,[Password]
--				   ,[Timeout]
--				   ,[Permission]
--				   ,[PersonId]
--				   ,[UniqueId]
--				   ,[SID]
--				   ,[LastLogin]
--				   ,[ImageId]
--				   ,[Enabled]
--				   ,[DeletedBy]
--				   ,[CreatedBy]
--				   ,[DateCreated])
--            VALUES
--				   (@Login
--				   ,''
--				   ,1000
--				   ,@Permission
--				   ,@personId
--				   ,NEWID()
--				   ,@SID
--				   ,SYSDATETIME()
--				   ,NULL
--				   ,1
--				   ,NULL
--				   ,1
--				   ,SYSDATETIME())

--			SET @existingSid = @SID
--			SET @userId = SCOPE_IDENTITY()

--			INSERT INTO BORevision([Table],BOId,BOName,[Date],UserId,Comment,Json,[Type])
--			VALUES('User',@userId,'',dbo.GetZoneDate(),1,'AD insert user','',1)
--		END

--		IF @existingSid IS NULL
--		BEGIN
--			UPDATE [User]
--			SET
--				[SID] = @SID
--			WHERE
--				[UserId] = @userId

--			SET @existingSid = @SID

--			INSERT INTO BORevision([Table],BOId,BOName,[Date],UserId,Comment,Json,[Type])
--			VALUES('User',@userId,'',dbo.GetZoneDate(),1,'AD update user','',1)
--		END
		
--		SELECT u.UserId,u.Login,u.Timeout,u.UniqueId,u.Permission, u.Password,u.DateCreated,u.LastLogin,u.CreatedBy,u.DisplayError
--				,r.RoleId, r.Name as RoleName, r.Permission as RolePermission
--				,p.PersonId, p.FirstName as PersonFirstName, p.LastName as PersonLastName, p.BirthDate as PersonBirthDate, p.Email as PersonEmail
--				,ug.GraphicId AS ImageId,ug.BOName AS ImageBOName,ug.Name AS ImageName,ug.Ext AS ImageExt
--				,rg.GraphicId AS AvatarId,rg.BOName AS AvatarBOName,rg.Name AS AvatarName,rg.Ext AS AvatarExt
--		FROM [User] u
--		INNER JOIN [Role] r ON r.RoleId=u.RoleId
--		LEFT JOIN [Person] p ON p.PersonId=u.PersonId
--		LEFT JOIN [Graphic] ug ON ug.GraphicId=u.ImageId
--		LEFT JOIN [Graphic] rg ON rg.GraphicId=r.AvatarId
--		WHERE u.[UserId] = @userId
		
--	END
