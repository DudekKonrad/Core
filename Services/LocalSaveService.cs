using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Application.Core.Services
{
    public class LocalSaveService : ISaveService
    {
        private readonly string _saveDirectory;
        private readonly byte[] _encryptionKey = Encoding.UTF8.GetBytes("F8eH2jK9xV4pL1wM5qZ8nR3tY7cU0bA6"); // 32 bytes for AES-256
        private readonly byte[] _encryptionIv = Encoding.UTF8.GetBytes("L1wM5qZ8nR3tY7cU"); // 16 bytes for AES

        public LocalSaveService()
        {
            _saveDirectory = UnityEngine.Application.persistentDataPath;
        }

        public void Save<T>(string key, T data)
        {
            try
            {
                var filePath = GetFilePath(key);
                var json = JsonUtility.ToJson(data);
                var encryptedData = Encrypt(json);
                File.WriteAllText(filePath, encryptedData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving data for key {key}: {e.Message}");
            }
        }

        public T Load<T>(string key, T defaultValue = default)
        {
            var filePath = GetFilePath(key);
            if (!File.Exists(filePath))
            {
                return defaultValue;
            }

            try
            {
                var encryptedData = File.ReadAllText(filePath);
                var decryptedJson = Decrypt(encryptedData);
                var data = JsonUtility.FromJson<T>(decryptedJson);
                return data != null ? data : defaultValue;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading data for key {key}: {e.Message}");
                return defaultValue;
            }
        }

        private string GetFilePath(string key)
        {
            return Path.Combine(_saveDirectory, $"{key}.sav");
        }

        private string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _encryptionKey;
            aes.IV = _encryptionIv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(plainText);
            }

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        private string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = _encryptionKey;
            aes.IV = _encryptionIv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }
    }
}