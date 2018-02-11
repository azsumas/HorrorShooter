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
    public Image HDResButton;
    public Image MidResButton;
    public Image FullResButton;
    public PostProcessingProfile postProcessingAsset;
    public Slider basicExposure;
    public GameObject TextOn;
    public GameObject TextOff;

    void Start()
    {

    }

    public void OnFullScreenOn()
    {
        Screen.fullScreen = !Screen.fullScreen;
        if (Screen.fullScreen == true)
        {
            TextOn.SetActive(false);
            TextOff.SetActive(true);
        }
        else
        {
            TextOn.SetActive(true);
            TextOff.SetActive(false);
        }
    }

    public void FullHDRes()
    {
        FullResButton.GetComponent<Image>().color = new Color32(165, 165, 255, 255);
        MidResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        HDResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }

    public void MidFullRes()
    {
        FullResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        MidResButton.GetComponent<Image>().color = new Color32(165, 165, 255, 255);
        HDResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        Screen.SetResolution(1600, 1200, Screen.fullScreen);
    }

    public void HDRes()
    {
        FullResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        MidResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        HDResButton.GetComponent<Image>().color = new Color32(165, 165, 255, 255);
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
