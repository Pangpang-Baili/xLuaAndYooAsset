using System.Collections;
using System.Collections.Generic;
using UniFramework.Machine;
using UnityEngine;
using YooAsset;

public class FsmInitializePackage : IStateNode
{
    private StateMachine _machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        _machine = machine;
    }

    void IStateNode.OnEnter()
    {
        PatchEventDefine.PatchStepChange.SendEventMessage("初始化资源包！");
        GameManager.Instance.StartCoroutine(InitPackage());
    }

    void IStateNode.OnExit()
    {

    }

    void IStateNode.OnUpdate()
    {
    }

    IEnumerator InitPackage()
    {
        var playMode = (EPlayMode)_machine.GetBlackboardValue("PlayMode");
        var packageName = (string)_machine.GetBlackboardValue("PackageName");

        var package = YooAssets.TryGetPackage(packageName);
        if (package == null)
            package = YooAssets.CreatePackage(packageName);

        InitializationOperation initializationOperation = null;
        if (playMode == EPlayMode.EditorSimulateMode)
        {
            //编辑器下模拟模式
            var buildResult = EditorSimulateModeHelper.SimulateBuild("xLuaAndYooAssets");
            var packageRoot = buildResult.PackageRootDirectory;
            var initParameters = new EditorSimulateModeParameters();
            initParameters.EditorFileSystemParameters = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
            initializationOperation = package.InitializeAsync(initParameters);
        }

        // 单机运行模式
        if (playMode == EPlayMode.OfflinePlayMode)
        {
            var createParameters = new OfflinePlayModeParameters();
            createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            initializationOperation = package.InitializeAsync(createParameters);
        }

        if (playMode == EPlayMode.HostPlayMode)
        {
            string defaultHostServer = GetHostServerURL(packageName);
            string fallbackHostServer = GetHostServerURL(packageName);
            IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            var initParameters = new HostPlayModeParameters();
            initParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters(); ;
            initParameters.CacheFileSystemParameters = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices); ;
            initializationOperation = package.InitializeAsync(initParameters);
        }
        yield return initializationOperation;

        if (initializationOperation.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning($"{initializationOperation.Error}");
            PatchEventDefine.InitializeFailed.SendEventMessage();
        }
        else
        {
            _machine.ChangeState<FsmRequestPackageVersion>();
        }
    }

    private string GetHostServerURL(string packageName)
    {
        return $"http://192.168.10.10/Resourece/{packageName}";
    }

    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    private class RemoteServices : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }

        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }

        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }

}
