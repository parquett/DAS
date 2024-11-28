--CREATE PROCEDURE [dbo].[Person_Populate_ByIDNP](
--	@IDNP NVARCHAR(20)
--)
--AS	
--	BEGIN
		
--		SELECT TOP 1 p.*
--		FROM Person p
--		WHERE p.IDNP = @IDNP
--		AND p.DeletedBy iS NULL

--	END