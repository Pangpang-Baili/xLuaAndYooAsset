using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using YooAsset;

public class AESBundleDecryption : IDecryptionServices
{
    DecryptResult IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo)
    {
        byte[] fileData = File.ReadAllBytes(fileInfo.FileLoadPath);
        byte[] decryptedData = AESDecrypt(fileData, "19943071377");
        DecryptResult decryptResult = new DecryptResult();
        decryptResult.ManagedStream = null;
        decryptResult.Result = AssetBundle.LoadFromMemory(decryptedData, fileInfo.FileLoadCRC);
        return decryptResult;
    }

    DecryptResult IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo)
    {
        byte[] fileData = File.ReadAllBytes(fileInfo.FileLoadPath);
        byte[] decryptedData = AESDecrypt(fileData, "19943071377");
        DecryptResult decryptResult = new DecryptResult();
        decryptResult.ManagedStream = null;
        decryptResult.CreateRequest = AssetBundle.LoadFromMemoryAsync(decryptedData, fileInfo.FileLoadCRC);
        return decryptResult;
    }

    byte[] IDecryptionServices.ReadFileData(DecryptFileInfo fileInfo)
    {
        throw new System.NotImplementedException();
    }

    string IDecryptionServices.ReadFileText(DecryptFileInfo fileInfo)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="DecryptString">待解密密文</param>
    /// <param name="DecryptKey">解密密钥</param>
    public byte[] AESDecrypt(byte[] DecryptByte, string DecryptKey)
    {
        if (DecryptByte.Length == 0) { throw (new Exception("密文不得为空")); }
        if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }
        byte[] m_strDecrypt;
        byte[] m_btIV = Convert.FromBase64String("AAAAAAAAAAAAAAAAAAAAAA==");
        byte[] m_salt = Convert.FromBase64String("bbbbbbbbbbbbbbbbbbbbbb==");
        Rijndael m_AESProvider = Rijndael.Create();
        try
        {
            MemoryStream m_stream = new MemoryStream();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(DecryptKey, m_salt);
            ICryptoTransform transform = m_AESProvider.CreateDecryptor(pdb.GetBytes(32), m_btIV);
            CryptoStream m_csstream = new CryptoStream(m_stream, transform, CryptoStreamMode.Write);
            m_csstream.Write(DecryptByte, 0, DecryptByte.Length);
            m_csstream.FlushFinalBlock();
            m_strDecrypt = m_stream.ToArray();
            m_stream.Close(); m_stream.Dispose();
            m_csstream.Close(); m_csstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_AESProvider.Clear(); }
        return m_strDecrypt;
    }
}