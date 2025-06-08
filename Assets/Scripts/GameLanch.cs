using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;
using YooAsset;

public class GameLanch : MonoSingleton<GameLanch>
{
    private List<IManager> _managers = new List<IManager>();
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
    protected override void Awake()
    {
        base.Awake();

        Debug.Log($"资源系统运行模式：{PlayMode}");
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator Start()
    {
        //y游戏管理器
        GameManager.Instance.Behaviour = this;

        //初始化事件系统
        UniEvent.Initalize();

        //初始化资源系统
        YooAssets.Initialize();

        var go = Resources.Load<GameObject>("PatchWindow");
        GameObject.Instantiate(go);

        var operation = new PatchOperation("xLuaAndYooAssets", PlayMode);
        YooAssets.StartOperation(operation);
        yield return operation;

        var gamePackage = YooAssets.GetPackage("xLuaAndYooAssets");
        YooAssets.SetDefaultPackage(gamePackage);

        ModelInit();
    }

    private void ModelInit()
    {
        _managers.Add(GameManager.Instance);
        _managers.Add(ResourceManager.Instance);
        _managers.Add(SceneManager.Instance);

        foreach (var manager in _managers)
        {
            manager.Init();
        }

        SceneEventDefine.ChangeScene.SendEventMessage("MainScene");
    }


}
