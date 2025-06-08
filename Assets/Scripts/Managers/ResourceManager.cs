using UnityEditor;
using UnityEngine;
using XLua;
using YooAsset;
using UObject = UnityEngine.Object;

[LuaCallCSharp]
public class ResourceManager : MonoSingleton<ResourceManager>
{
    private ResourcePackage _defaultPackage;
    public bool isInitialized = false;

    protected override void Awake()
    {
        base.Awake();
        _defaultPackage = YooAssets.GetPackage("xLuaAndYooAssets");
        if (_defaultPackage == null)
        {
            Debug.LogError("资源包不存在 :" + _defaultPackage);
        }

        isInitialized = _defaultPackage != null;
    }

    #region 资源加载
    public GameObject LoadAsset(string location)
    {
        if (AppConst.PlayMode == EPlayMode.EditorSimulateMode)
        {
#if UNITY_EDITOR
            string path = "Assets/GameResources/" + location;
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (asset == null)
            {
                Debug.LogError($"资源不存在：{path}");
                return null;
            }
            return asset;
#endif
        }
        else if (AppConst.PlayMode == EPlayMode.HostPlayMode)
        {
            string loadPath = "Assets/GameResources/" + location;
            AssetInfo assetInfo = YooAssets.GetAssetInfo(loadPath);
            if (assetInfo == null)
            {
                Debug.LogError($"资源不存在：{loadPath}");
                return null;
            }

            AssetHandle assetHandle = YooAssets.LoadAssetSync<GameObject>(loadPath);
            if (assetHandle == null || !assetHandle.IsValid)
            {
                Debug.LogError($"加载资源失败：{loadPath}");
                return null;
            }

            return assetHandle.AssetObject as GameObject;
        }
    }

    public TextAsset LoadFile(string filePath)
    {
        string loadPath = "Assets/GameResources/Game/" + filePath;
        Debug.Log($"加载文件路径：{loadPath}");
        var rawFileHandle = YooAssets.LoadAssetSync<TextAsset>(loadPath);
        if (rawFileHandle == null)
        {
            Debug.Log($"加载文件失败：{loadPath}");
            return null;
        }

        return rawFileHandle.AssetObject as TextAsset;
    }

    public void LoadSceneAsync(string sceneName)
    {
        if (isInitialized)
        {
            YooAssets.LoadSceneAsync(sceneName);
        }
        else
        {
            Debug.LogError("资源包未初始化");
        }
    }
    #endregion
}
