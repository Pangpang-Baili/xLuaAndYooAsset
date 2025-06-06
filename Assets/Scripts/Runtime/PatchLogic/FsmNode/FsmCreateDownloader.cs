using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UniFramework.Machine;
using UnityEngine;
using YooAsset;

public class FsmCreateDownloader : IStateNode
{
    private StateMachine _machine;
    public void OnCreate(StateMachine machine)
    {
        _machine = machine;
    }

    public void OnEnter()
    {
        PatchEventDefine.PatchStepChange.SendEventMessage("创建资源下载器！");
        CreateDownloader();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }

    private void CreateDownloader()
    {
        var packageName = (string)_machine.GetBlackboardValue("PackageName");
        var package = YooAssets.GetPackage(packageName);
        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
        _machine.SetBlackboardValue("Downloader", downloader);

        if (downloader.TotalDownloadCount == 0)
        {
            Debug.Log("Not found any download files ! ");
            _machine.ChangeState<FsmStartGame>();
        }
        else
        {
            int totalDownloadCount = downloader.TotalDownloadCount;
            long totalDownloadBytes = downloader.TotalDownloadBytes;

            //检测磁盘空间是否充足
            long freeSpace = DiskSpaceChecker.GetFreeSpaceBytes();
            if (freeSpace < totalDownloadBytes)
            {
                Debug.LogWarning("FreeSpace not enough! ");
            }
            PatchEventDefine.FoundUpdateFiles.SendEventMessage(totalDownloadCount, totalDownloadBytes);
        }
    }


}
