using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;


public class SceneEventDefine
{
    public class ChangeScene : IEventMessage
    {
        public string sceneName;

        public ChangeScene(string sceneName)
        {
            this.sceneName = sceneName;
        }

        public static void SendEventMessage(string sceneName)
        {
            var msg = new ChangeScene(sceneName);
            UniEvent.SendMessage(msg);
        }
    }


}
