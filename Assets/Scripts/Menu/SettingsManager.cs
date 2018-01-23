using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;

public class SettingsManager : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public Dropdown qualityDrop;
    public Button applyButton;
    public AudioMixer audioMixer;
    public Image HDResButton;
    public Image MidResButton;
    public Image FullResButton;
    public PostProcessingProfile postProcessingAsset;
    public Slider basicExposure;

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
        Debug.Log("asas");
    }

    public void OnQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.quality = qualityDrop.value;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void PostProcessing()
    {
        ColorGradingModel.Settings gradientSettings = postProcessingAsset.colorGrading.settings;
        gradientSettings.basic.postExposure = basicExposure.value;
        postProcessingAsset.colorGrading.settings = gradientSettings;
    }
}
