using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Argus.Backend.Utility
{
    public enum CryptoAlgorithm
    {
        Aes = 0,
        DES,
        RC2,
        Rijndael,
        TripleDES,
    }

    public class Encryptor
    {
        private readonly ICryptoTransform encryptor;
        private readonly ICryptoTransform decryptor;
        private const int BUFFER_SIZE = 1024;

        public Encryptor(CryptoAlgorithm algorithm, byte[] key)
        {
            SymmetricAlgorithm provider = SymmetricAlgorithm.Create(algorithm.ToString());
            provider.Key = key;
            provider.IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            encryptor = provider.CreateEncryptor();
            decryptor = provider.CreateDecryptor();
        }

        // 加密算法
        public string Encrypt(string clearText)
        {
            // 创建明文流
            byte[] clearBuffer = Encoding.UTF8.GetBytes(clearText);
            MemoryStream clearStream = new MemoryStream(clearBuffer);

            // 创建空的密文流
            MemoryStream encryptedStream = new MemoryStream();

            CryptoStream cryptoStream =
            new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write);

            // 将明文流写入到buffer中
            // 将buffer中的数据写入到cryptoStream中
            int bytesRead;
            byte[] buffer = new byte[BUFFER_SIZE];
            do
            {
                bytesRead = clearStream.Read(buffer, 0, BUFFER_SIZE);
                cryptoStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            cryptoStream.FlushFinalBlock();

            // 获取加密后的文本
            buffer = encryptedStream.ToArray();
            string encryptedText = Convert.ToBase64String(buffer);
            return encryptedText;
        }

        // 解密算法
        public string Decrypt(string encryptedText)
        {
            byte[] encryptedBuffer = Convert.FromBase64String(encryptedText);
            Stream encryptedStream = new MemoryStream(encryptedBuffer);


            MemoryStream clearStream = new MemoryStream();
            CryptoStream cryptoStream =
            new CryptoStream(encryptedStream, decryptor, CryptoStreamMode.Read);

            int bytesRead;
            byte[] buffer = new byte[BUFFER_SIZE];

            do
            {
                bytesRead = cryptoStream.Read(buffer, 0, BUFFER_SIZE);
                clearStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            buffer = clearStream.GetBuffer();
            string clearText =
            Encoding.UTF8.GetString(buffer, 0, (int)clearStream.Length);

            return clearText;
        }

        public static string Encrypt(string clearText, string key)
        {
            byte[] keyData = new byte[16];
            byte[] sourceData = Encoding.Default.GetBytes(key);
            int copyBytes = 16;
            if (sourceData.Length < 16) copyBytes = sourceData.Length;

            Array.Copy(sourceData, keyData, copyBytes);

            Encryptor encryptor = new Encryptor(CryptoAlgorithm.Aes, keyData);
            return encryptor.Encrypt(clearText);
        }

        public static string Decrypt(string encryptedText, string key)
        {
            byte[] keyData = new byte[16];
            byte[] sourceData = Encoding.Default.GetBytes(key);
            int copyBytes = 16;
            if (sourceData.Length < 16) copyBytes = sourceData.Length;

            Array.Copy(sourceData, keyData, copyBytes);

            Encryptor decryptor = new Encryptor(CryptoAlgorithm.Aes, keyData);
            return decryptor.Decrypt(encryptedText);
        }
    }
}
