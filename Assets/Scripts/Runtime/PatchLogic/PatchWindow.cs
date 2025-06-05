using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;
using UnityEngine.UI;

public class PatchWindow : MonoBehaviour
{
    private class MessageBox
    {
        private GameObject _cloneObject;
        private Text _content;
        private Button _btnOK;
        private System.Action _clickOK;

        public bool ActiveSelf
        {
            get { return _cloneObject.activeSelf; }
        }

        public void Create(GameObject cloneObject)
        {
            _cloneObject = cloneObject;
            _content = cloneObject.transform.Find("txt_content").GetComponent<Text>();
            _btnOK = cloneObject.transform.Find("btn_ok").GetComponent<Button>();
            _btnOK.onClick.AddListener(OnClickYes);
        }

        public void Show(string content, System.Action clickOK)
        {
            _content.text = content;
            _clickOK = clickOK;
            _cloneObject.SetActive(true);
            _cloneObject.transform.SetAsLastSibling();
        }

        public void Hide()
        {
            _content.text = string.Empty;
            _clickOK = null;
            _cloneObject.SetActive(false);
        }

        private void OnClickYes()
        {
            _clickOK?.Invoke();
            Hide();
        }
    }

    private readonly EventGroup _eventGroup = new EventGroup();
    private readonly List<MessageBox> _msgBoxList = new List<MessageBox>();

    private GameObject _messageBoxObj;
    private Slider _slider;
    private Text _tips;

    void Awake()
    {
        _slider = transform.Find("UIWindow/Slider").GetComponent<Slider>();
        _tips = transform.Find("UIWindow/Slider/txt_tips").GetComponent<Text>();
        _tips.text = "Initializing the game world !";
        _messageBoxObj = transform.Find("UIWindow/MessgeBox").gameObject;
        _messageBoxObj.SetActive(false);
    }

    void OnDestroy()
    {
        _eventGroup.RemoveAllListener();
    }

    private void OnHandleEventMessage(IEventMessage message)
    {
        if (message is PatchEventDefine.InitializeFailed)
        {
            System.Action callback = () =>
            {
                UserEventDefine.UserTryInitailize.SendEventMessage();
            };
            ShowMessageBox($"Failed to initialize package !", callback);
        }
        else if (message is PatchEventDefine.PatchStepChange)
        {
            var msg = message as PatchEventDefine.PatchStepChange;
            _tips.text = msg.Tips;
            Debug.Log(msg.Tips);
        }
    }

    private void ShowMessageBox(string content, System.Action ok)
    {
        MessageBox msgBox = null;

        for (int i = 0; i < _msgBoxList.Count; i++)
        {
            var item = _msgBoxList[i];
            if (item.ActiveSelf == false)
            {
                msgBox = item;
                break;
            }
        }

        if (msgBox == null)
        {
            msgBox = new MessageBox();
            var cloneObject = GameObject.Instantiate(
                _messageBoxObj,
                _messageBoxObj.transform.parent
            );
            msgBox.Create(cloneObject);
            _msgBoxList.Add(msgBox);
        }

        msgBox.Show(content, ok);
    }
}
