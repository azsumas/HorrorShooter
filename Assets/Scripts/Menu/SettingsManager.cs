using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingsManager : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public Dropdown resolutionDrop;
    public Dropdown qualityDrop;
    public Button applyButton;

    public Resolution[] resolutions;
    public GameSettings gameSettings;

    void Start()
    {
        OnFullScreenChange();
        resolutionDrop.RefreshShownValue();
    }
    void OnEnable()
    {
        gameSettings = new GameSettings();

        fullScreenToggle.onValueChanged.AddListener(delegate { OnFullScreenChange(); });
        resolutionDrop.onValueChanged.AddListener(delegate { OnResulitionChange(); });
        qualityDrop.onValueChanged.AddListener(delegate { OnQualityChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });

        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions)
        {
            resolutionDrop.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
        //LoadSettings();
    }

    public void OnFullScreenChange()
    {
        gameSettings.fullScreen =  Screen.fullScreen = fullScreenToggle.isOn;
    }

    public void OnResulitionChange()
    {
        Screen.SetResolution(resolutions[resolutionDrop.value].width, resolutions[resolutionDrop.value].height, Screen.fullScreen);
        gameSettings.resolution = resolutionDrop.value;
    }

    public void OnQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.quality = qualityDrop.value;
    }

    public void OnApplyButtonClick()
    {
        //SaveSettings();
    }

   /* public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesttings.json", jsonData);
    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));

        fullScreenToggle.isOn = gameSettings.fullScreen;
        resolutionDrop.value = gameSettings.resolution;
        qualityDrop.value = gameSettings.resolution;
    }*/
}
