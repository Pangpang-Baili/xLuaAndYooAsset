using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset;

/// <summary>
/// 文件流加密方式：针对资源文件的二进制数据进行逐字异或加密
/// </summary>
public class FileStreamEncryption : IEncryptionServices
{
    //对传入文件数据进行加密操作
    public EncryptResult Encrypt(EncryptFileInfo fileInfo)
    {
        // 检查文件名是否包含"_resource_"，仅对资源包文件进行加密 需要注意的是必须是assetbundle文件整个路径是这样的assets_gameresources_resource_uifont.bundle
        if (fileInfo.BundleName.Contains("_resource_"))
        {
            var fileData = File.ReadAllBytes(fileInfo.FileLoadPath);
            for (int i = 0; i < fileData.Length; i++)
            {
                fileData[i] ^= 32;
            }

            EncryptResult result = new EncryptResult();
            result.Encrypted = true;
            result.EncryptedData = fileData;
            return result;
        }
        else
        {
            EncryptResult result = new EncryptResult();
            result.Encrypted = false;
            return result;
        }
    }


}

/// <summary>
/// 文件偏移加密方式：在资源文件开头添加固定字节偏移量
/// 注意：这种方式仅适用于资源包文件，且需要在加载时跳过
/// </summary>
public class FileOffsetEncrypt : IEncryptionServices
{
    public EncryptResult Encrypt(EncryptFileInfo fileInfo)
    {
        // 检查文件名是否包含"_resource_"，仅对资源包文件进行加密 需要注意的是必须是assetbundle文件整个路径是这样的assets_gameresources_resource_uifont.bundle
        if (fileInfo.BundleName.Contains("_resource_"))
        {
            Debug.Log("FileOffsetEncrypt: " + fileInfo.BundleName);
            int offset = 32;
            byte[] fileData = File.ReadAllBytes(fileInfo.FileLoadPath);
            var encryptedData = new byte[fileData.Length + offset];

            Buffer.BlockCopy(fileData, 0, encryptedData, offset, fileData.Length);

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
}

/// <summary>
/// 资源文件解密流
/// </summary>
public class BundleStream : FileStream
{
    public const byte KEY = 64;

    public BundleStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access, share)
    {
    }
    public BundleStream(string path, FileMode mode) : base(path, mode)
    {
    }

    public override int Read(byte[] array, int offset, int count)
    {
        var index = base.Read(array, offset, count);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] ^= KEY;
        }
        return index;
    }
}

