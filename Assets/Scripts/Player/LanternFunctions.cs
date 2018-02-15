using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFunctions : MonoBehaviour 
{
    public bool switchOn;
    public Light lantern;
    public GameObject managerScene;
    private AudioPlayer audioPlayer;

    private void Awake()
    {
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
        this.gameObject.SetActive(false);
    }

    public void SwitchOn() 
	{
        switchOn = !switchOn;
        if (switchOn)
        {
            gameObject.SetActive(true);
            if((object)audioPlayer != null) audioPlayer.PlaySFX(8);
        }
        else
        {
            gameObject.SetActive(false);
            if((object)audioPlayer != null) audioPlayer.PlaySFX(9);
        }
    }
}
