--CREATE PROCEDURE [dbo].[User_DoLoginByCNP]
--(
--	@CNP nvarchar(50)
--)

--AS
--	BEGIN
--		SET NOCOUNT ON;
		
--		SELECT u.UserId,u.Login,u.Timeout,u.UniqueId,u.Permission, u.Password,u.DateCreated,u.LastLogin,u.CreatedBy
--				,r.RoleId, r.Name as RoleName, r.Permission as RolePermission
--				,p.PersonId, p.FirstName as PersonFirstName, p.LastName as PersonLastName, p.BirthDate as PersonBirthDate, p.Email as PersonEmail
--				,ug.GraphicId AS ImageId,ug.BOName AS ImageBOName,ug.Name AS ImageName,ug.Ext AS ImageExt
--				,rg.GraphicId AS AvatarId,rg.BOName AS AvatarBOName,rg.Name AS AvatarName,rg.Ext AS AvatarExt
--		FROM [User] u
--		INNER JOIN [Role] r ON r.RoleId=u.RoleId
--		INNER JOIN [Person] p ON p.PersonId=u.PersonId
--		LEFT JOIN [Graphic] ug ON ug.GraphicId=u.ImageId
--		LEFT JOIN [Graphic] rg ON rg.GraphicId=r.AvatarId
--		WHERE p.IDNP=@CNP and u.Enabled = 1
		
--	END
