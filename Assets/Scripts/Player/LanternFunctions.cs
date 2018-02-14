using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFunctions : MonoBehaviour 
{
    public bool switchOn;
    public Light lantern;
    public GameObject managerScene;
    AudioPlayer audioPlayer;

    private void Start()
    {
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
    }

    public void SwitchOn () 
	{
        switchOn = !switchOn;

        if (switchOn)
        {
            gameObject.SetActive(true);
            audioPlayer.PlaySFX(8);
        }
        else
        {
            gameObject.SetActive(false);
            audioPlayer.PlaySFX(9);
        }
    }
}
