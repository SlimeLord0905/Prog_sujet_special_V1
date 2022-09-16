using System.Security.Cryptography;
using System.Text;

namespace CryptoLib;

public class AesCrypter
{
    private Aes _aes = Aes.Create();

    //TODO: make the key property private?
    public string Key
    {
        get => string.Join("", _aes.Key.Select(b => $"{b:X}"));
        set => _aes.Key = SHA256.HashData(Encoding.ASCII.GetBytes(value));
    }

    public AesCrypter()
    {
    }

    public AesCrypter(String key)
    {
        Key = key;
    }

    public void EncryptToFile(String data, String path)
    {
        using (FileStream fileStream = new(path, FileMode.OpenOrCreate))
        {
            byte[] iv = _aes.IV;
            fileStream.Write(iv, 0, iv.Length);

            using (CryptoStream cryptoStream = new(
                       fileStream,
                       _aes.CreateEncryptor(),
                       CryptoStreamMode.Write))
            {
                using (StreamWriter encryptWriter = new(cryptoStream))
                {
                    encryptWriter.WriteLine(data);
                }
            }
        }
    }

    public string DecryptFromFile(String path)
    {
        using (FileStream fileStream = new(path, FileMode.Open))
        {
            byte[] iv = new byte[_aes.IV.Length];
            int numBytesToRead = _aes.IV.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                if (n == 0) break;

                numBytesRead += n;
                numBytesToRead -= n;
            }


            using (CryptoStream cryptoStream = new(
                       fileStream,
                       _aes.CreateDecryptor(_aes.Key, iv),
                       CryptoStreamMode.Read))
            {
                using (StreamReader decryptReader = new(cryptoStream))
                {
                    string decryptedMessage = decryptReader.ReadToEnd();
                    return decryptedMessage;
                }
            }
        }
    }

    public string Encrypt(String data)
    {
        using (MemoryStream memoryStream = new())
        {
            byte[] iv = _aes.IV;
            memoryStream.Write(iv, 0, iv.Length);

            byte[] encryptedData;
            using (CryptoStream cryptoStream = new(
                       memoryStream,
                       _aes.CreateEncryptor(),
                       CryptoStreamMode.Write))
            {
                using (StreamWriter encryptWriter = new(cryptoStream))
                {
                    encryptWriter.WriteLine(data);
                }

                encryptedData = memoryStream.ToArray();
            }

            return Convert.ToBase64String(encryptedData);
        }
    }

    public string Decrypt(string encrypted)
    {
        using (MemoryStream memoryStream = new(Convert.FromBase64String(encrypted)))
        {
            byte[] iv = new byte[_aes.IV.Length];
            int numBytesToRead = _aes.IV.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                int n = memoryStream.Read(iv, numBytesRead, numBytesToRead);
                if (n == 0) break;

                numBytesRead += n;
                numBytesToRead -= n;
            }


            using (CryptoStream cryptoStream = new(
                       memoryStream,
                       _aes.CreateDecryptor(_aes.Key, iv),
                       CryptoStreamMode.Read))
            {
                using (StreamReader decryptReader = new(cryptoStream))
                {
                    string decryptedMessage = decryptReader.ReadToEnd();
                    return decryptedMessage;
                }
            }
        }
    }
}