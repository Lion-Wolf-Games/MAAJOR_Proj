using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SoundSettingsManager : MonoBehaviour
{
    public AK.Wwise.RTPC MasterVolume;
    public AK.Wwise.RTPC MusicVolume;
    public AK.Wwise.RTPC SFXVolume;

    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    //[SerializeField] private TextMeshProUGUI _masterText;
    //[SerializeField] private TextMeshProUGUI _musicText;
    //[SerializeField] private TextMeshProUGUI _sfxText;

    private void Awake()
    {
        float masterV = PlayerPrefs.GetFloat("MasterVolume", 100);
        float musicV = PlayerPrefs.GetFloat("MusicVolume", 100);
        float sfxV = PlayerPrefs.GetFloat("SfxVolume", 100);

        SetVolume(masterV, MasterVolume);
        SetVolume(musicV, MusicVolume);
        SetVolume(sfxV, SFXVolume);

        _masterSlider.SetValueWithoutNotify(masterV);
        _musicSlider.SetValueWithoutNotify(musicV);
        _sfxSlider.SetValueWithoutNotify(sfxV);

        //_masterText.text = masterV.ToString();
        //_musicText.text = musicV.ToString();
        //_sfxText.text = sfxV.ToString();
    }

    private void Start()
    {
        SettingsManager.Instance.OnApply += SetSoundSettings;
        SettingsManager.Instance.OnCancel += CancelSoundSettings;
    }

    private void SetVolume(float value, AK.Wwise.RTPC rtpc)
    {
        rtpc.SetGlobalValue(value);
    }

    public void SetMaster(float value)
    {
        SetVolume(value, MasterVolume);
        //_masterText.text = value.ToString();
    }

    public void SetMusic(float value)
    {
        SetVolume(value, MusicVolume);
        //_musicText.text = value.ToString();
    }

    public void SetSFX(float value)
    {
        SetVolume(value, SFXVolume);
        //_sfxText.text = value.ToString();
    }

    public void SetSoundSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", _masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);
        PlayerPrefs.SetFloat("SfxVolume", _sfxSlider.value);
    }

    public void CancelSoundSettings()
    {
        SetMaster(PlayerPrefs.GetFloat("MasterVolume"));
        SetMusic(PlayerPrefs.GetFloat("MusicVolume"));
        SetSFX(PlayerPrefs.GetFloat("SfxVolume"));
    } 
}
