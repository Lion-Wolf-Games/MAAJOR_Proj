using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenSettingsManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    private int resolutionIndex;
    public TMP_Dropdown qualityDropdown;
    private int qualityIndex;
    private bool isFullScreen;
    public Toggle fullScreenToggle;
    Resolution[] resolutions;

    public Toggle vSyncToggle;
    private bool isVSync;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        SettingsManager.Instance.OnApply += SetScreenSettings;
    }

    private void OnEnable()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        fullScreenToggle.isOn = Screen.fullScreen;

        

        if (QualitySettings.vSyncCount != 0)
        {
            vSyncToggle.isOn = true;
        }
        else
        {
            vSyncToggle.isOn = false;
        }
    }

    public void ChangeResolutionIndex(int index)
    {
        resolutionIndex = resolutionDropdown.value;
    }

    public void ChangeQualityIndex(int index)
    {
        qualityIndex = index;
    }

    public void ChangeVsync(bool isOn)
    {
        isVSync = isOn;
    }

    public void SetScreenSettings()
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        if (fullScreenToggle.isOn != Screen.fullScreen)
        {
            Screen.fullScreen = fullScreenToggle.isOn;
        }

        if (QualitySettings.GetQualityLevel() != qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        if (isVSync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Debug.Log("Applying resolution: " + resolution.width + "x" + resolution.height);
    }

}
