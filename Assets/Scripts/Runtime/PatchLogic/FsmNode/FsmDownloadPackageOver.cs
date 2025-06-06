using System.Collections;
using System.Collections.Generic;
using UniFramework.Machine;
using UnityEngine;

public class FsmDownloadPackageOver : IStateNode
{
    private StateMachine _machine;
    public void OnCreate(StateMachine machine)
    {
        _machine = machine;
    }

    public void OnEnter()
    {
        PatchEventDefine.PatchStepChange.SendEventMessage("资源文件下载完毕！");
        _machine.ChangeState<FsmClearCacheBundle>();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }

}
