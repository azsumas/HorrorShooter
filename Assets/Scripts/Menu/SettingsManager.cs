using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public Dropdown qualityDrop;
    public Button applyButton;
    public AudioMixer audioMixer;

    public GameSettings gameSettings;

    void Start()
    {
        OnFullScreenChange();
        HDRes();
    }
    void OnEnable()
    {
        gameSettings = new GameSettings();

        fullScreenToggle.onValueChanged.AddListener(delegate { OnFullScreenChange(); });
        qualityDrop.onValueChanged.AddListener(delegate { OnQualityChange(); });
    }

    public void OnFullScreenChange()
    {
        gameSettings.fullScreen =  Screen.fullScreen = fullScreenToggle.isOn;
    }

    public void FullHDRes()
    {
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }

    public void MidFullRes()
    {
        Screen.SetResolution(1600, 1200, Screen.fullScreen);
    }

    public void HDRes()
    {
        Screen.SetResolution(1280, 720, Screen.fullScreen);
    }

    public void OnQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.quality = qualityDrop.value;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}
