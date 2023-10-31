using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLib
{
    public class SymmetricCryptoUtility
    {
        // Symmetric Encryption with IV
        public static byte[] SymmetricEncryptWithIv(byte[] plaintext, byte[] key, byte[] iv, bool withIv = false)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] encrypted = encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);
                    return withIv ? Combine(iv, encrypted) : encrypted;
                }
            }
        }

        // Symmetric Decryption with IV
        public static byte[] SymmetricDecryptWithIv(byte[] encrypted, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
                }
            }
        }

        // Symmetric Encryption without explicit IV
        public static byte[] SymmetricEncrypt(byte[] plaintext, byte[] key)
        {
            byte[] iv = GenerateRandomBytes(16);
            return SymmetricEncryptWithIv(plaintext, key, iv, true);
        }

        // Symmetric Decryption without explicit IV
        public static byte[] SymmetricDecrypt(byte[] encrypted, byte[] key)
        {
            byte[] iv = new byte[16];
            Array.Copy(encrypted, 0, iv, 0, 16);
            byte[] actualEncrypted = new byte[encrypted.Length - 16];
            Array.Copy(encrypted, 16, actualEncrypted, 0, encrypted.Length - 16);
            return SymmetricDecryptWithIv(actualEncrypted, key, iv);
        }

        // Utility method to combine IV and encrypted data
        private static byte[] Combine(byte[] iv, byte[] encrypted)
        {
            byte[] combined = new byte[iv.Length + encrypted.Length];
            Array.Copy(iv, 0, combined, 0, iv.Length);
            Array.Copy(encrypted, 0, combined, iv.Length, encrypted.Length);
            return combined;
        }

        // Utility method to generate random bytes
        private static byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
