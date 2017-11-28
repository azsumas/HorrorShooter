using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFunctions : MonoBehaviour 
{
    public bool switchOn;
    public Light lantern;

    public void SwitchOn () 
	{
        switchOn = !switchOn;

        if (switchOn)
        {
            gameObject.SetActive(true);
            //Debug.Log("Lantern Switch ON");
        }
        else
        {
            gameObject.SetActive(false);
            //Debug.Log("Lantern Switch OFF");
        }
    }
}
