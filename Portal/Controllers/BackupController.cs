using Microsoft.AspNetCore.Mvc;
using Lib.Tools.Utils;
using System.IO;
using System;

namespace SecurityCRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackupController : ControllerBase
    {
        [HttpPost("backup")]
        public IActionResult BackupFile([FromQuery] string filePath, [FromQuery] string password)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(password))
            {
                return BadRequest("File path and password are required.");
            }

            try
            {
                BackupManager.BackupFile(filePath, password);
                return Ok("Backup completed successfully.");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("restore")]
        public IActionResult RestoreFile([FromQuery] string backupFilePath, [FromQuery] string restorePath, [FromQuery] string password)
        {
            if (string.IsNullOrEmpty(backupFilePath) || string.IsNullOrEmpty(restorePath) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Backup file path, restore path, and password are required.");
            }

            try
            {
                BackupManager.RestoreFile(backupFilePath, restorePath, password);
                return Ok("Restore completed successfully.");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
