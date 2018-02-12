using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animator anim;
    public GameObject managerScene;
    AudioPlayer audioPlayer;

    void Start()
    {
        anim.GetComponent<Animator>();
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
    }

    void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("OpenDoor");
        audioPlayer.PlaySFX(11);
    }

    private void OnTriggerExit(Collider other)
    {
        anim.enabled = true;
        audioPlayer.PlaySFX(11);
    }

    void PauseAnimationEvent()
    {
        anim.enabled = false;
    }
}
