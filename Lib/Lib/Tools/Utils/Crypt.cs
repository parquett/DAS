// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Crypt.cs" company="SecurityCRM">
//   Copyright �  2020
// </copyright>
// <summary>
//   The encoding decoding.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.Tools.Utils
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// The encoding decoding.
    /// </summary>
    public class Crypt
    {
        // Encrypt a byte array into a byte array using a key and an IV 

        /// <summary>
        /// The encrypt.
        /// </summary>
        /// <param name="clearData">
        /// The clear data.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="iv">
        /// The iv.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] Encrypt(byte[] clearData, byte[] key, byte[] iv)
        {
            // Create a MemoryStream to accept the encrypted bytes 

            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 

            // We are going to use Rijndael because it is strong and

            // available on all platforms. 

            // You can use other algorithms, to do so substitute the

            // next line with something like 

            //      TripleDES alg = TripleDES.Create(); 

            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 

            // We need the IV (Initialization Vector) because

            // the algorithm is operating in its default 

            // mode called CBC (Cipher Block Chaining).

            // The IV is XORed with the first block (8 byte) 

            // of the data before it is encrypted, and then each

            // encrypted block is XORed with the 

            // following block of plaintext.

            // This is done to make encryption more secure. 


            // There is also a mode called ECB which does not need an IV,

            // but it is much less secure. 

            alg.Key = key;
            alg.IV = iv;

            // Create a CryptoStream through which we are going to be

            // pumping our data. 

            // CryptoStreamMode.Write means that we are going to be

            // writing data to the stream and the output will be written

            // in the MemoryStream we have provided. 

            CryptoStream cs = new CryptoStream(ms,
                alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 

            cs.Write(clearData, 0, clearData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 

            // This will tell it that we have done our encryption and

            // there is no more data coming in, 

            // and it is now a good time to apply the padding and

            // finalize the encryption process. 

            cs.Close();

            // Now get the encrypted data from the MemoryStream.

            // Some people make a mistake of using GetBuffer() here,

            // which is not the right way. 

            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }


        /// <summary>
        /// Encrypt a string into a string using a password 
        /// Uses Encrypt(byte[], byte[], byte[]) 
        /// </summary>
        /// <param name="clearText">
        /// The clear text.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Encrypt(string clearText, string password)
        {
            // First we need to turn the input string into a byte array. 

            byte[] clearBytes =
                System.Text.Encoding.Unicode.GetBytes(clearText);

            // Then, we need to turn the password into Key and IV 

            // We are using salt to make it harder to guess our key

            // using a dictionary attack - 

            // trying to guess a password by enumerating all possible words. 

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            // Now get the key/IV and do the encryption using the

            // function that accepts byte arrays. 

            // Using PasswordDeriveBytes object we are first getting

            // 32 bytes for the Key 

            // (the default Rijndael key length is 256bit = 32bytes)

            // and then 16 bytes for the IV. 

            // IV should always be the block size, which is by default

            // 16 bytes (128 bit) for Rijndael. 

            // If you are using DES/TripleDES/RC2 the block size is

            // 8 bytes and so should be the IV size. 

            // You can also read KeySize/BlockSize properties off

            // the algorithm to find out the sizes. 

            byte[] encryptedData = Encrypt(clearBytes,
                pdb.GetBytes(32), pdb.GetBytes(16));

            // Now we need to turn the resulting byte array into a string. 

            // A common mistake would be to use an Encoding class for that.

            //It does not work because not all byte values can be

            // represented by characters. 

            // We are going to be using Base64 encoding that is designed

            //exactly for what we are trying to do. 

            return Convert.ToBase64String(encryptedData);

        }
        
        /// <summary>
        /// Encrypt a file into another file using a password 
        /// Decrypt a byte array into a byte array using a key and an IV 
        /// </summary>
        /// <param name="cipherData">
        /// The cipher data.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="iv">
        /// The iv.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] Decrypt(byte[] cipherData,byte[] key, byte[] iv)
        {
            // Create a MemoryStream that is going to accept the

            // decrypted bytes 

            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 

            // We are going to use Rijndael because it is strong and

            // available on all platforms. 

            // You can use other algorithms, to do so substitute the next

            // line with something like 

            //     TripleDES alg = TripleDES.Create(); 

            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 

            // We need the IV (Initialization Vector) because the algorithm

            // is operating in its default 

            // mode called CBC (Cipher Block Chaining). The IV is XORed with

            // the first block (8 byte) 

            // of the data after it is decrypted, and then each decrypted

            // block is XORed with the previous 

            // cipher block. This is done to make encryption more secure. 

            // There is also a mode called ECB which does not need an IV,

            // but it is much less secure. 

            alg.Key = key;
            alg.IV = iv;

            // Create a CryptoStream through which we are going to be

            // pumping our data. 

            // CryptoStreamMode.Write means that we are going to be

            // writing data to the stream 

            // and the output will be written in the MemoryStream

            // we have provided. 

            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the decryption 

            cs.Write(cipherData, 0, cipherData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 

            // This will tell it that we have done our decryption

            // and there is no more data coming in, 

            // and it is now a good time to remove the padding

            // and finalize the decryption process. 

            cs.Close();

            // Now get the decrypted data from the MemoryStream. 

            // Some people make a mistake of using GetBuffer() here,

            // which is not the right way. 

            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }


        /// <summary>
        /// Decrypt a string into a string using a password 
        ///    Uses Decrypt(byte[], byte[], byte[]) 
        /// </summary>
        /// <param name="cipherText">
        /// The cipher text.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Decrypt(string cipherText, string password)
        {
            // First we need to turn the input string into a byte array. 

            // We presume that Base64 encoding was used 
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Then, we need to turn the password into Key and IV 

                // We are using salt to make it harder to guess our key

                // using a dictionary attack - 

                // trying to guess a password by enumerating all possible words. 

                PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
                                0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

                // Now get the key/IV and do the decryption using

                // the function that accepts byte arrays. 

                // Using PasswordDeriveBytes object we are first

                // getting 32 bytes for the Key 

                // (the default Rijndael key length is 256bit = 32bytes)

                // and then 16 bytes for the IV. 

                // IV should always be the block size, which is by

                // default 16 bytes (128 bit) for Rijndael. 

                // If you are using DES/TripleDES/RC2 the block size is

                // 8 bytes and so should be the IV size. 

                // You can also read KeySize/BlockSize properties off

                // the algorithm to find out the sizes. 

                byte[] decryptedData = Decrypt(cipherBytes,
                    pdb.GetBytes(32), pdb.GetBytes(16));

                // Now we need to turn the resulting byte array into a string. 

                // A common mistake would be to use an Encoding class for that.

                // It does not work 

                // because not all byte values can be represented by characters. 

                // We are going to be using Base64 encoding that is 

                // designed exactly for what we are trying to do. 

                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());
                return cipherText;
            }
        }

        public static byte[] Encrypt(byte[] clearData, string password)
        {
            using (var pdb = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
            {
                using (var alg = Aes.Create())
                {
                    alg.Key = pdb.GetBytes(32);
                    alg.IV = pdb.GetBytes(16);

                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearData, 0, clearData.Length);
                            cs.Close();
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

        public static byte[] Decrypt(byte[] cipherData, string password)
        {
            using (var pdb = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
            {
                using (var alg = Aes.Create())
                {
                    alg.Key = pdb.GetBytes(32);
                    alg.IV = pdb.GetBytes(16);

                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherData, 0, cipherData.Length);
                            cs.Close();
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

    }
}