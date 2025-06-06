using System.Collections;
using System.Collections.Generic;
using UniFramework.Machine;
using UnityEngine;
using YooAsset;

public class FsmDownloadPackageFiles : IStateNode
{
    private StateMachine _machine;
    public void OnCreate(StateMachine machine)
    {
        _machine = machine;
    }

    public void OnEnter()
    {
        PatchEventDefine.PatchStepChange.SendEventMessage("开始下载资源文件！");
        GameManager.Instance.StartCoroutine(BeginDownload());
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }

    private IEnumerator BeginDownload()
    {
        var downloader = (ResourceDownloaderOperation)_machine.GetBlackboardValue("Downloader");
        downloader.DownloadErrorCallback = PatchEventDefine.WebFileDownloadFailed.SendEventMessage;
        downloader.DownloadUpdateCallback = PatchEventDefine.DownloadUpdate.SendEventMessage;
        downloader.BeginDownload();
        yield return downloader;

        // 检测下载结果
        if (downloader.Status != EOperationStatus.Succeed)
            yield break;

        _machine.ChangeState<FsmDownloadPackageOver>();
    }

}
