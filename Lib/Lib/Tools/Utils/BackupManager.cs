using System;
using System.IO;
using System.Security.Cryptography;

namespace Lib.Tools.Utils
{
    public static class BackupManager
    {
        private static readonly string BackupDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");

        public static void BackupFile(string filePath, string password)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            Directory.CreateDirectory(BackupDirectory);

            string backupFilePath = Path.Combine(BackupDirectory, Path.GetFileName(filePath) + ".bak");

            byte[] fileBytes = File.ReadAllBytes(filePath);
            byte[] encryptedBytes = Crypt.Encrypt(fileBytes, password);

            File.WriteAllBytes(backupFilePath, encryptedBytes);
        }

        public static void RestoreFile(string backupFilePath, string restorePath, string password)
        {
            if (!File.Exists(backupFilePath))
                throw new FileNotFoundException("Backup file not found", backupFilePath);

            byte[] encryptedBytes = File.ReadAllBytes(backupFilePath);
            byte[] decryptedBytes = Crypt.Decrypt(encryptedBytes, password);

            File.WriteAllBytes(restorePath, decryptedBytes);
        }
    }
}
