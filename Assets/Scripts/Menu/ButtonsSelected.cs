using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSelected : MonoBehaviour
{
    public Image buttonOptions;
    public Image buttonExit;

    public Image buttonLanguage;
    public Image buttonSettings;
    public Image buttonControls;

    private void Start()
    {
        SettingsSelected();
    }

    public void LanguageSelected()
    {
        buttonLanguage.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
        buttonSettings.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        buttonControls.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
    public void SettingsSelected()
    {
        buttonLanguage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        buttonSettings.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
        buttonControls.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
    public void ControlsSelected()
    {
        buttonLanguage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        buttonSettings.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        buttonControls.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
    }

    public void OptionsSelected ()
    {
        buttonOptions.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
    }
    public void OptionsDeselected()
    {
        buttonOptions.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void ExitSelected()
    {
        buttonExit.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
    }
    public void ExitDeselected()
    {
        buttonExit.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
}
