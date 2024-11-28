BEGIN
	DBCC CHECKIDENT ('dbo.Permission', RESEED, 0);
	SET IDENTITY_INSERT [dbo].[Permission] ON 
	-- TBD
	SET IDENTITY_INSERT [dbo].[Permission] OFF
END 
GO
