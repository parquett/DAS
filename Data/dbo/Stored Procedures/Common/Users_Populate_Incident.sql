CREATE PROCEDURE [dbo].[Users_Populate_Incident]
AS
BEGIN
SELECT 
    i.IncidentId,
    MAX(i.[Name]) AS Name, 
    COUNT(i.IncidentId) AS IncidentCount
FROM 
    [dbo].[Incident] i
WHERE 
    i.DeletedBy IS NULL 
GROUP BY 
    i.IncidentId;
END;
