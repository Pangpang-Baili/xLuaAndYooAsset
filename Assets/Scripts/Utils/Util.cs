using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    /// <summary>
    /// 生成指定长度的随机盐值或初始向量
    /// </summary>
    /// <param name="length">字节长度，常用16字节（128位）</param>
    /// <returns>随机字节数组</returns>
    public static byte[] GenerateRandomBytes(int length)
    {
        byte[] randomBytes = new byte[length];
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }
        return randomBytes;
    }
}
