using Business.Genisis.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Business.Genisis.Encryption
{
    public class AesEncryptionService : IEncryption
    {

	    //Please note that this is not the Encryption we're using in Prod XD XD XD.
        //The dummy encryption will probably break the code.

        #region Public Methods
        public string OneWayHashEncryption(string plainText)
        {
            using (Rfc2898DeriveBytes rfc2898 = new(plainText, Encoding.UTF8.GetBytes("ASDFASDQweraD3EQW2R3214ASD"), 10000))
                return Convert.ToBase64String(rfc2898.GetBytes(20));
        }

        public string Encrypt(string clearText)
        {
            string EncryptionKey = "ASDF2143231ASDF";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x43, 0x66, 0x64, 0x6e, 0x4d, 0x4d, 0x35, 0x64, 0x76, 0x85, 0x64, 0x95, 0x16 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "ASDRFGQERT121234";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x43, 0x66, 0x64, 0x6e, 0x4d, 0x4d, 0x35, 0x64, 0x76, 0x85, 0x64, 0x95, 0x16 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion
    }
}