using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLantern : MonoBehaviour
{
    public GameObject lanternText;
    public GameObject managerScene;
    public AudioPlayer audioPlayer;

    public void Start()
    {
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
    }

    void OnTriggerEnter(Collider other)
    {
        lanternText.SetActive(true);
        audioPlayer.PlaySFX(2);
    }

    private void OnTriggerExit(Collider other)
    {
        lanternText.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
