using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCrouch : MonoBehaviour
{
    public GameObject crouchText;
    public GameObject managerScene;
    public AudioPlayer audioPlayer;

    public void Start()
    {
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
    }

    void OnTriggerEnter(Collider other)
    {
        crouchText.SetActive(true);
        audioPlayer.PlaySFX(2);
    }

    private void OnTriggerExit(Collider other)
    {
        crouchText.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
