using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsMain : MonoBehaviour
{
    public GameObject managerScene;
    public AudioPlayer musicMain;

	// Use this for initialization
	void Start ()
    {
        managerScene = GameObject.FindWithTag("Manager");
        musicMain = managerScene.GetComponentInChildren<AudioPlayer>();
        Play();
    }
	
	// Update is called once per frame
	void Play ()
    {
        musicMain.PlayMusic(0, 0.02f, true);
    }

    public void Stop()
    {
        musicMain.StopMusic();
    }

    public void SelectSound()
    {
        musicMain.PlaySFX(0);
    }

    public void BackSound()
    {
        musicMain.PlaySFX(1);
    }
}
