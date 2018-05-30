using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTutorial : MonoBehaviour
{
    public GameObject aimText;
    public GameObject managerScene;
    public AudioPlayer audioPlayer;

    public void Start()
    {
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
    }

    void OnTriggerEnter(Collider other)
    {
        aimText.SetActive(true);
        audioPlayer.PlaySFX(22);
    }

    private void OnTriggerExit(Collider other)
    {
        aimText.SetActive(false);
        this.gameObject.SetActive(false);
    }
}