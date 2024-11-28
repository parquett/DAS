CREATE PROCEDURE [dbo].[Person_Populate_Autocomplete](
	@search nvarchar(50),
	@SectionId bigint = NULL,
	@UserId bigint=NULL
)
AS	
	BEGIN
		
		SELECT TOP 50 p.PersonId,p.FirstName,p.LastName
		FROM Person p
		INNER JOIN [User] u ON u.PersonId=p.PersonId
		WHERE  p.DeletedBy iS NULL 
		AND (
			(p.FirstName like '%'+@search+'%')
			OR 
			(p.LastName like '%'+@search+'%')
		)
		AND (
			(@UserId IS NULL) OR (@UserId IS NOT NULL AND u.UserId=@UserId)
		)
	END
