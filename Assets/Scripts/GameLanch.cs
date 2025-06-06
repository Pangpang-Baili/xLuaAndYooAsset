using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;
using YooAsset;

public class GameLanch : MonoSingleton<GameLanch>
{
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
    protected override void Awake()
    {
        base.Awake();

        Debug.Log($"资源系统运行模式：{PlayMode}");
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);

        // this.gameObject.AddComponent<xLuaManager>();
        // this.gameObject.AddComponent<ResourceManager>();
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

        var operation = new PatchOperation("DefaultPackage", PlayMode);
        YooAssets.StartOperation(operation);
        yield return operation;

        var gamePackage = YooAssets.GetPackage("DefaultPackage");
        YooAssets.SetDefaultPackage(gamePackage);
    }

    // IEnumerator GameStart()
    // {
    //     yield return this.StartCoroutine(checkHotUpdate());
    //     xLuaManager.Instance.EnterGame();
    // }

    // IEnumerator checkHotUpdate()
    // {
    //     while (!ResourceManager.Instance.isInitialized)
    //     {
    //         yield return null;
    //     }
    // }
}
