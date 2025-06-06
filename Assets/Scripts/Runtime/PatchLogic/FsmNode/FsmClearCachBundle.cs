using System.Collections;
using System.Collections.Generic;
using UniFramework.Machine;
using UnityEngine;
using YooAsset;

public class FsmClearCacheBundle : IStateNode
{
    private StateMachine _machine;

    public void OnCreate(StateMachine machine)
    {
        _machine = machine;
    }

    public void OnEnter()
    {
        PatchEventDefine.PatchStepChange.SendEventMessage("清理未使用的缓存文件！");
        var packageName = (string)_machine.GetBlackboardValue("PackageName");
        var package = YooAssets.GetPackage(packageName);
        var operation = package.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
        operation.Completed += Operation_Completed;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }

    private void Operation_Completed(YooAsset.AsyncOperationBase obj)
    {
        _machine.ChangeState<FsmStartGame>();
    }

}
