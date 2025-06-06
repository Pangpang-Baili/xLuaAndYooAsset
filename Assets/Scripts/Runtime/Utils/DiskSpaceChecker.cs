using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;

public class DiskSpaceChecker
{
    // 获取指定路径的剩余空间（字节）
    public static long GetFreeSpaceBytes(string path = "")
    {
        if (string.IsNullOrEmpty(path))
        {
            path = Application.persistentDataPath;
        }

        try
        {
            if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return GetWindowsFreeSpace(path);
            }
            // else if (Application.platform == RuntimePlatform.Android)
            // {
            //     return GetAndroidFreeSpace(path);
            // }
            // else if (Application.platform == RuntimePlatform.OSXEditor ||
            //          Application.platform == RuntimePlatform.OSXPlayer)
            // {
            //     return GetMacFreeSpace(path);
            // }
            // else if (Application.platform == RuntimePlatform.IPhonePlayer)
            // {
            //     return GetIosFreeSpace(path);
            // }
            // else if (Application.platform == RuntimePlatform.LinuxPlayer)
            // {
            //     return GetLinuxFreeSpace(path);
            // }
            else if (Application.isEditor)
            {
                // 编辑器默认使用Windows路径
                return GetWindowsFreeSpace(Application.dataPath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"获取磁盘空间失败: {e.Message}");
        }
        return -1;
    }

    // 计算总下载文件大小
    public static long CalculateTotalDownloadSize(List<FileInfo> files)
    {
        long total = 0;
        foreach (var file in files)
        {
            total += file.Length;
        }
        return total;
    }

    // 平台具体实现
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetDiskFreeSpaceEx(
        string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);

    private static long GetWindowsFreeSpace(string path)
    {
        ulong freeBytesAvailable;
        ulong totalNumberOfBytes;
        ulong totalNumberOfFreeBytes;

        if (GetDiskFreeSpaceEx(path, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes))
        {
            return (long)freeBytesAvailable;
        }
        return -1;
    }
#endif

#if UNITY_ANDROID
    private static long GetAndroidFreeSpace(string path)
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (AndroidJavaObject env = currentActivity.Call<AndroidJavaObject>("getAssets"))
        {
            string dataPath = env.Call<AndroidJavaObject>("getAbsolutePath").ToString();
            AndroidJavaObject statFs = new AndroidJavaObject("android.os.StatFs", dataPath);
            
            long blockSize = statFs.Call<long>("getBlockSizeLong");
            long availableBlocks = statFs.Call<long>("getAvailableBlocksLong");
            
            return blockSize * availableBlocks;
        }
    }
#endif

#if UNITY_IOS
    private static long GetIosFreeSpace(string path)
    {
        // iOS需要使用原生插件，此处为简化实现
        string documentsPath = Application.dataPath + "/Documents";
        return GetMacFreeSpace(documentsPath);
    }
#endif

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    private static long GetMacFreeSpace(string path)
    {
        var drive = new DriveInfo(Path.GetPathRoot(path));
        return drive.AvailableFreeSpace;
    }
#endif

#if UNITY_STANDALONE_LINUX
    private static long GetLinuxFreeSpace(string path)
    {
        DriveInfo drive = new DriveInfo(Path.GetPathRoot(path));
        return drive.AvailableFreeSpace;
    }
#endif
}
