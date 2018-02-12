using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedDoor : MonoBehaviour
{
    bool enter;
    public GameObject text;
    public GameObject managerScene;
    AudioPlayer audioPlayer;

    private void Start()
    {
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
    }

    void OnTriggerStay(Collider other)
    {
        if (enter == true) return;
        enter = true;
        text.SetActive(true);
        audioPlayer.PlaySFX(12);
    }

    private void OnTriggerExit(Collider other)
    {
        enter = false;
        text.SetActive(false);
    }
}
