DELETE FROM Permission WHERE PermissionId>17

if not exists (select p.PermissionId from Permission p where p.PermissionId = 18 and p.[Value] = 131072)
begin
	SET IDENTITY_INSERT dbo.Permission ON;  
		insert into Permission (PermissionId, [Name], [Value], CreatedBy, DateCreated)
		values (18, 'API Access',131072,1,dbo.GetZoneDate())
	SET IDENTITY_INSERT dbo.Permission OFF;  

end

