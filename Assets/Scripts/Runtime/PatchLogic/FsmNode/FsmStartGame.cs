using System.Collections;
using System.Collections.Generic;
using UniFramework.Machine;
using UnityEngine;

public class FsmStartGame : IStateNode
{
    private PatchOperation _owner;
    public void OnCreate(StateMachine machine)
    {
        _owner = machine.Owner as PatchOperation;
    }

    public void OnEnter()
    {
        PatchEventDefine.PatchStepChange.SendEventMessage("开始游戏！");
        _owner.SetFinish();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }

}
