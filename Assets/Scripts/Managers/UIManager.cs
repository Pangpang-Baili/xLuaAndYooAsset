using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private Transform _uiRoot;

    protected override void Awake()
    {
        base.Awake();
        GameObject _uiRootObj = Resources.Load<GameObject>("Prefabs/UI/UIRoot");
        _uiRoot = Instantiate(_uiRootObj).transform;
    }

    public GameObject ShowUI(string uiName)
    {
        GameObject uiPrefab = Resources.Load<GameObject>($"Prefabs/UI/{uiName}");
        if (uiPrefab != null)
        {
            GameObject uiInstance = Instantiate(uiPrefab, _uiRoot);
            uiInstance.name = uiName;
            return uiInstance;
        }
        else
        {
            Debug.LogError($"UI {uiName} not found!");
            return null;
        }
    }
}
