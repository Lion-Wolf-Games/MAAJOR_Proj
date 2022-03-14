using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenSettingsManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    private int resolutionIndex;
    private int qualityIndex;
    private bool isFullScreen;
    public Toggle fullScreenToggle;
    Resolution[] resolutions;

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

        fullScreenToggle.isOn = Screen.fullScreen;
    }

    public void ChangeResolutionIndex(int index)
    {
        resolutionIndex = resolutionDropdown.value;
    }

    public void ChangeQualityIndex(int index)
    {
        qualityIndex = index;
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

        Debug.Log("Applying resolution: " + resolution.width + "x" + resolution.height);
    }

}
