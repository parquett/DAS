--CREATE PROCEDURE [dbo].[User_DoLogin_Token]
--(
--	@Token nvarchar(50)
--)

--AS
--	BEGIN
--		SET NOCOUNT ON;
		
--		SELECT u.UserId,u.Login,u.Timeout,u.Permission, u.Password,u.DisplayError
--				,r.RoleId, r.Name as RoleName, r.Permission as RolePermission
--				,p.PersonId, p.FirstName as PersonFirstName, p.LastName as PersonLastName, p.BirthDate as PersonBirthDate, p.Email as PersonEmail
--		FROM [User] u
--		INNER JOIN [Role] r ON r.RoleId=u.RoleId
--		INNER JOIN Token t ON t.UserId=u.UserId AND t.TokenStateId=1
--		LEFT JOIN [Person] p ON p.PersonId=u.PersonId
--		WHERE t.Code=@Token and u.Enabled = 1
		
--	END
