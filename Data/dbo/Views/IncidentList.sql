Create VIEW [dbo].[IncidentList]
AS
SELECT 
    i.IncidentId,
    i.Name As [Name],
    i.DateCreated,
    i.CreatedBy
FROM 
    [Incident] i
WHERE 
    DeletedBy IS NULL;
