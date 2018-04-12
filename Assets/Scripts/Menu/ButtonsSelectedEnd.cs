using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSelectedEnd : MonoBehaviour
{
    public Image buttonExit;

    public void ExitSelected()
    {
        buttonExit.GetComponent<Image>().color = new Color32(49, 137, 100, 255);
    }
    public void ExitDeselected()
    {
        buttonExit.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
}


