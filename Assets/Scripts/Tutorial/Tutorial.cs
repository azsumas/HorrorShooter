using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject textInitialization;
    public GameObject player;
    private inputManeger controlPlayer;
    public GameObject ammoText;
    public GameObject packEneText;
    public GameObject energyBar;

    public GameObject hudAmmo;
    public Image ammoImage;

    public GameObject radar;
    private Image radarMask;

    public float timeCounter;

    public GameObject managerScene;
    public AudioPlayer audioPlayer;
    bool playSound = false;
    bool playSound2 = false;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindWithTag("Player");
        controlPlayer = player.GetComponentInChildren<inputManeger>();

        hudAmmo = GameObject.FindWithTag("AmmoEnergy");
        ammoImage = hudAmmo.GetComponentInChildren<Image>();

        radar = GameObject.FindWithTag("Radar");
        radarMask = radar.GetComponentInChildren<Image>();

        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= 0.5f) StartSound();
        if (timeCounter >= 3.0f) radarMask.color = new Vector4(75, 75, 75, 220);
        if (timeCounter >= 4.0f) ammoImage.color = new Vector4(255, 255, 255, 255);
        if (timeCounter >= 4.5f) PlaySound();
        if (timeCounter >= 5.5f) ammoText.SetActive(true);
        if (timeCounter >= 6.0f) packEneText.SetActive(true);
        if (timeCounter >= 7.0f) energyBar.SetActive(true);
        if(timeCounter >= 8.2f)
        {
            controlPlayer.enabled = true;
            this.gameObject.SetActive(false);
        }
    }

    public void PlaySound()
    {
        if (playSound == true) return;
        audioPlayer.PlaySFX(19);
        playSound = true;
    }

    public void StartSound()
    {
        if (playSound2 == true) return;
        audioPlayer.PlaySFX(25, 1, 1.05f,false,false,"SFX");
        playSound2 = true;
    }
}
