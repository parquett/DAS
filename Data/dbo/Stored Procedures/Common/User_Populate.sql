CREATE PROCEDURE [dbo].[User_Populate](	
	@UserID bigint
)
AS
	
	BEGIN

		SELECT u.UserId,u.Login,u.UniqueId,u.RoleId,r.Name as RoleName, r.Permission as RolePermissions,  u.PersonId
				,u.DateCreated,u.LastLogin,u.CreatedBy,p.FirstName as PersonFirstName,p.LastName as PersonLastName
		FROM [User] u
		INNER JOIN [Person] p ON p.PersonId=u.PersonId
		INNER JOIN [Role] r ON r.RoleId=u.RoleId
		WHERE u.UserId=@UserID 

	END