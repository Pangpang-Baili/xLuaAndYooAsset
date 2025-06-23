using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset;

public class FileStreamDecryption : IDecryptionServices
{
    DecryptResult IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo)
    {
        BundleStream bundleStream = new BundleStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        DecryptResult decryptResult = new DecryptResult();
        decryptResult.ManagedStream = bundleStream;
        decryptResult.Result = AssetBundle.LoadFromStream(bundleStream, fileInfo.FileLoadCRC, GetManagedReadBufferSize());
        return decryptResult;

    }

    DecryptResult IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo)
    {
        BundleStream bundleStream = new BundleStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        DecryptResult decryptResult = new DecryptResult();
        decryptResult.ManagedStream = bundleStream;
        decryptResult.CreateRequest = AssetBundle.LoadFromStreamAsync(bundleStream, fileInfo.FileLoadCRC, GetManagedReadBufferSize());
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

    private static uint GetManagedReadBufferSize()
    {
        return 1024;
    }
}

public class FileOffsetDecryption : IDecryptionServices
{
    DecryptResult IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo)
    {
        // 构建解密结果对象，ManagedStream 设为 null（无自定义流）
        DecryptResult decryptResult = new DecryptResult();
        decryptResult.ManagedStream = null;
        // 从文件中同步加载资源包，跳过文件偏移部分
        decryptResult.Result = AssetBundle.LoadFromFile(fileInfo.FileLoadPath, fileInfo.FileLoadCRC, GetFileOffset());
        return decryptResult;
    }

    /// <summary>
    /// 异步方式加载资源包对象
    /// 注意：该方式直接通过文件路径加载资源包，加载时会跳过文件开头的偏移数据
    /// </summary>
    /// <param name="fileInfo">包含资源包加载路径、CRC校验码等信息</param>
    /// <returns>返回包含异步加载请求的 DecryptResult 对象</returns>
    DecryptResult IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo)
    {
        // 构建解密结果对象，ManagedStream 设为 null（无自定义流）
        DecryptResult decryptResult = new DecryptResult();
        decryptResult.ManagedStream = null;
        // 从文件中异步加载资源包，跳过文件偏移部分
        decryptResult.CreateRequest =
            AssetBundle.LoadFromFileAsync(fileInfo.FileLoadPath, fileInfo.FileLoadCRC, GetFileOffset());
        return decryptResult;
    }


    /// <summary>
    /// 获取解密后的字节数据
    /// 当前未实现该方法，调用时会抛出 NotImplementedException 异常
    /// </summary>
    /// <param name="fileInfo">包含解密文件相关信息</param>
    /// <returns>解密后的字节数组</returns>
    byte[] IDecryptionServices.ReadFileData(DecryptFileInfo fileInfo)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 获取解密后的文本数据
    /// 当前未实现该方法，调用时会抛出 NotImplementedException 异常
    /// </summary>
    /// <param name="fileInfo">包含解密文件相关信息</param>
    /// <returns>解密后的文本字符串</returns>
    string IDecryptionServices.ReadFileText(DecryptFileInfo fileInfo)
    {
        throw new System.NotImplementedException();
    }


    // <summary>
    /// 获取文件偏移量，表示资源包文件开头需要跳过的字节数
    /// </summary>
    /// <returns>偏移字节数</returns>
    private static ulong GetFileOffset()
    {
        return 64;
    }
}
