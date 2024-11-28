CREATE PROCEDURE [dbo].[App_DoLogin_API]
(
	@AppId bigint,
	@Secret nvarchar(200)
)

AS
	BEGIN
		SET NOCOUNT ON;
		
		SELECT u.UserId,u.Login,u.Timeout,u.Permission, u.Password,u.DisplayError
				,r.RoleId, r.Name as RoleName, r.Permission as RolePermission
				,p.PersonId, p.FirstName as PersonFirstName, p.LastName as PersonLastName, p.BirthDate as PersonBirthDate, p.Email as PersonEmail
		FROM [User] u
		INNER JOIN [Role] r ON r.RoleId=u.RoleId
		INNER JOIN APIApplication app ON app.UserId=u.UserId
		LEFT JOIN [Person] p ON p.PersonId=u.PersonId
		WHERE app.APIApplicationId=@AppId and app.Secret=@Secret and u.Enabled = 1
		
	END
