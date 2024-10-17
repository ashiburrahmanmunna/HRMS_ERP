using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GTERP.Models.Common
{
    public static class clsEncryption ///42 kb
    {
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        //private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("HR$2pIjHR$2pIj12");
        private static string strSecretCode = "$SGqTLr@2A0f1O4a$";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        public static string Encrypt(string Password)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(Password);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(strSecretCode, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string Password)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(Password);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(strSecretCode, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                symmetricKey.Padding = PaddingMode.None;
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }


        public static string EncryptTDES(string Password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();
            TripleDESCryptoServiceProvider tDES = new TripleDESCryptoServiceProvider();
            tDES.Key = md5.ComputeHash(utf8.GetBytes(strSecretCode));
            tDES.Mode = CipherMode.ECB;
            tDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform trans = tDES.CreateEncryptor();
            byte[] encrypted = trans.TransformFinalBlock(utf8.GetBytes(Password), 0, utf8.GetBytes(Password).Length);
            return BitConverter.ToString(encrypted);

        }

        public static string DecryptTDES(string Password)
        {
            String[] arr = Password.Split('-');
            byte[] encypted = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                encypted[i] = Convert.ToByte(arr[i], 16);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();
            TripleDESCryptoServiceProvider tDES = new TripleDESCryptoServiceProvider();
            tDES.Key = md5.ComputeHash(utf8.GetBytes(strSecretCode));
            tDES.Mode = CipherMode.ECB;
            tDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform trans = tDES.CreateDecryptor();
            return utf8.GetString(trans.TransformFinalBlock(encypted, 0, encypted.Length));
        }
    }
}
