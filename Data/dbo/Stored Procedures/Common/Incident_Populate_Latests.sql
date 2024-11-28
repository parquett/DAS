CREATE PROCEDURE [dbo].[Incident_Populate_Latests](
    @LastestId BIGINT = NULL
)
AS
BEGIN
    SELECT TOP 10
        i.IncidentId,
        i.Name,
        i.DeletedBy,
        i.CreatedBy,
        i.DateCreated,
        i.DateUpdated
    FROM
        Incident i
    LEFT JOIN
        [User] u ON i.CreatedBy = u.UserId
    WHERE
        i.DeletedBy IS NULL
        AND (@LastestId IS NULL OR i.IncidentId > @LastestId)
    ORDER BY
        i.IncidentId DESC;
END
