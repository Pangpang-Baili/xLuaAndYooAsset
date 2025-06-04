using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class UILoadingForm : MonoBehaviour
{
    [SerializeField]
    private Slider _progressBar;

    [SerializeField]
    private Text _progressText;

    private UpdatePackageManifestOperation _updateOperation;

    public void ShowUpdateUI()
    {
        _progressBar.value = 0;
        _progressText.text = "Checking for updates...";
    }

    public void UpdateProgress(string progressText, float progressValue)
    {
        _progressBar.value = progressValue;
        _progressText.text = progressText;
    }
}
