using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFunctions : MonoBehaviour 
{
    public bool switchOn;
    public PlayerBehaviour player;
    public Light lantern;

    void Update()
    {
        if (player.energy <= 50)
        {
            lantern.intensity -= (Time.deltaTime/20);
            Debug.Log("Low Battery");
        }
    }

    public void SwitchOn () 
	{
        switchOn = !switchOn;

        if (switchOn)
        {
            gameObject.SetActive(true);
            Debug.Log("Lantern Switch ON");
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
