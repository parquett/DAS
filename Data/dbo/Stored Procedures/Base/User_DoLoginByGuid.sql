CREATE PROCEDURE [dbo].[User_DoLoginByGuid]
(
	@unique_id nvarchar(50)
)

AS
	BEGIN
		SET NOCOUNT ON;
		
		SELECT u.UserId,u.Login,u.RoleId,  u.PersonId
				,u.DateCreated,u.LastLogin,u.CreatedBy
		FROM [User] u
		WHERE u.UniqueId=@unique_id 
		
	END