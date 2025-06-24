using System;
using System.IO;
using System.Security.Cryptography;
using YooAsset;

public class AESBundleEncryption : IEncryptionServices
{
    public EncryptResult Encrypt(EncryptFileInfo fileInfo)
    {
        if (fileInfo.BundleName.Contains("_resource_"))
        {
            byte[] fileData = File.ReadAllBytes(fileInfo.FileLoadPath);
            byte[] encryptedData = AESEncrypt(fileData, "19943071377");
            EncryptResult result = new EncryptResult();
            result.Encrypted = true;
            result.EncryptedData = encryptedData;
            return result;
        }
        else
        {
            EncryptResult result = new EncryptResult();
            result.Encrypted = false;
            return result;
        }
    }

    /// <summary>
    /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="EncryptString">待加密密文</param>
    /// <param name="EncryptKey">加密密钥</param>
    public byte[] AESEncrypt(byte[] EncryptBytes, string EncryptKey)
    {
        if (EncryptBytes.Length == 0)
            throw new System.Exception("EncryptBytes is empty!");

        if (string.IsNullOrEmpty(EncryptKey))
            throw new System.Exception("EncryptKey is empty!");

        byte[] m_strEncrypt;
        //定义初始向量
        byte[] m_btIV = Util.GenerateRandomBytes(16);  //Convert.FromBase64String("AAAAAAAAAAAAAAAAAAAAAA==");
        //定义盐值
        byte[] m_salt =  Util.GenerateRandomBytes(16); //Convert.FromBase64String("bbbbbbbbbbbbbbbbbbbbbb==");
        //创建一个Rijndael算法的加密服务提供者实例
        Rijndael m_AESProvider = Rijndael.Create(); 
        try
        {
            //创建内存流对象，用于存储加密后的数据
            MemoryStream m_stream = new MemoryStream();
            //通过加密密钥和盐值生成一个密钥派生字节对象，用于后续生成加密密钥
            PasswordDeriveBytes m_Password = new PasswordDeriveBytes(EncryptKey, m_salt);
            //用派生出来的32位字节密钥和IV创建一个加密器对象
            ICryptoTransform m_Encryptor = m_AESProvider.CreateEncryptor(m_Password.GetBytes(32), m_btIV);
            //创建一个加密流，将加密器和内存流关联起来，数据写入时会自动加密
            CryptoStream m_CryptoStream = new CryptoStream(m_stream, m_Encryptor, CryptoStreamMode.Write);
            //将待加密的字节数组写入加密流，进行加密处理
            m_CryptoStream.Write(EncryptBytes, 0, EncryptBytes.Length);
            //刷新加密流，确保所有数据都被加密并写入内存流
            m_CryptoStream.FlushFinalBlock();
            //将内存流中的加密数据转为字节数组，赋值给m_strEncrypt
            m_strEncrypt = m_stream.ToArray();
            //关闭并释放加密流和内存流资源
            m_CryptoStream.Close();
            m_stream.Close();
            m_CryptoStream.Dispose();
            m_stream.Dispose();
        }
        catch (IOException ex)
        {
            throw new System.Exception("AES加密失败！", ex);
        }
        catch (CryptographicException ex)
        {
            throw new System.Exception("AES加密失败！", ex);
        }
        catch (Exception ex)
        {
            throw new System.Exception("AES加密失败！", ex);
        }
        finally
        {
            m_AESProvider.Clear();
        }
        return m_strEncrypt;
    }
}
