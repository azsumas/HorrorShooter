using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;

public class SettingsManager : MonoBehaviour
{
    public Button applyButton;
    public AudioMixer audioMixer;
    public PostProcessingProfile postProcessingAsset;
    public Slider basicExposure;
    public GameObject TextOn;
    public GameObject TextOff;

    public void Update()
    {
        if (Screen.fullScreen)
        {
            TextOn.SetActive(true);
            TextOff.SetActive(false);
        }
        else if (!Screen.fullScreen)
        {
            TextOn.SetActive(false);
            TextOff.SetActive(true);
        }
    }

    public void OnFullScreenOn()
    {
        Screen.fullScreen = !Screen.fullScreen;
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

    public void ChangeQualityFast()
    {
        QualitySettings.SetQualityLevel(0, true);
    }

    public void ChangeQualityGood()
    {
        QualitySettings.SetQualityLevel(1, true);
    }

    public void ChangeQualityFantastic()
    {
        QualitySettings.SetQualityLevel(2, true);
    }

    public void ChangeQualityUltra()
    {
        QualitySettings.SetQualityLevel(3, true);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetAmbientVolume(float volume)
    {
        audioMixer.SetFloat("AmbientVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void PostProcessing()
    {
        ColorGradingModel.Settings gradientSettings = postProcessingAsset.colorGrading.settings;
        gradientSettings.basic.postExposure = basicExposure.value;
        postProcessingAsset.colorGrading.settings = gradientSettings;
    }
}
