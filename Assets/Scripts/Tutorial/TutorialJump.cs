using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialJump : MonoBehaviour
{
    public GameObject jumpText;
    public GameObject managerScene;
    public AudioPlayer audioPlayer;

    public void Start()
    {
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
    }

    void OnTriggerEnter(Collider other)
    {
        jumpText.SetActive(true);
        audioPlayer.PlaySFX(2);
    }

    private void OnTriggerExit(Collider other)
    {
        jumpText.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
