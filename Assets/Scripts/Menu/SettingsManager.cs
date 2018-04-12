using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public PostProcessingProfile postProcessingAsset;
    public Slider basicExposure;
    public GameObject textOn;
    public GameObject textOff;
    public Image hdResButton;
    public Image midResButton;
    public Image fullResButton;
    public Image fast;
    public Image good;
    public Image fantastic;
    public Image ultra;
    public Image spa;
    public Image eng;

    public void Update()
    {
        if (Screen.fullScreen)
        {
            textOn.SetActive(true);
            textOff.SetActive(false);
        }
        else if (!Screen.fullScreen)
        {
            textOn.SetActive(false);
            textOff.SetActive(true);
        }

        // RESOLUTION
        if(Screen.width == 1280)
        {
            fullResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            midResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            hdResButton.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
        }
        if(Screen.width == 1600)
        {
            fullResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            midResButton.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
            hdResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if(Screen.width == 1920)
        {
            fullResButton.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
            midResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            hdResButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        // QUALITY
        if(QualitySettings.GetQualityLevel() == 0)
        {
            fast.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
            good.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            fantastic.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            ultra.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (QualitySettings.GetQualityLevel() == 1)
        {
            fast.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            good.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
            fantastic.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            ultra.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (QualitySettings.GetQualityLevel() == 2)
        {
            fast.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            good.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            fantastic.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
            ultra.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (QualitySettings.GetQualityLevel() == 3)
        {
            fast.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            good.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            fantastic.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            ultra.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
        }

        if (Language.language == Language.Lang.esES)
        {
            spa.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
            eng.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (Language.language == Language.Lang.enUS)
        {
            eng.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
            spa.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
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
