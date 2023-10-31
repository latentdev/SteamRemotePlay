using System.Security.Cryptography;

namespace CryptoLib
{
    public class CryptoUtility
    {
        // RSA Encryption
        public static byte[] RsaEncrypt(byte[] plaintext, string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(publicKey);
                return rsa.Encrypt(plaintext, true);
            }
        }

        // RSA Decryption
        public static byte[] RsaDecrypt(byte[] ciphertext, string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(privateKey);
                return rsa.Decrypt(ciphertext, true);
            }
        }
    }
}