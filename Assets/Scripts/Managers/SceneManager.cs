using System.IO;
using UniFramework.Event;
using UnityEngine;

public class SceneManager : MonoSingleton<SceneManager>
{
    private readonly EventGroup _eventGroup = new EventGroup();

    protected override void Awake()
    {
        base.Awake();

        _eventGroup.AddListener<SceneEventDefine.ChangeScene>(OnHandleEventMessage);
    }

    public void OnHandleEventMessage(IEventMessage message)
    {
        if (message is SceneEventDefine.ChangeScene)
        {
            SceneEventDefine.ChangeScene changeScene = message as SceneEventDefine.ChangeScene;
            string loadPath = Path.Combine("Assets/Scenes/MainScene", changeScene.sceneName).Replace("\\", "/");
            ResourceManager.Instance.LoadSceneAsync(loadPath);
        }
    }
}
