using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPack : MonoBehaviour
{
    public int energyPack;
    public Image icon;
    public GameObject managerScene;
    AudioPlayer audioPlayer;

    private void Start()
    {
        Radar.RegisterRadarObject(this.gameObject, icon);
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.GetComponent<PlayerBehaviour>().energy < 100)
        {
            other.GetComponent<PlayerBehaviour>().RecoveryEnergy(energyPack);

            Radar.RemoveRadarObject(this.gameObject);

            this.gameObject.SetActive(false);
        }*/
        other.GetComponent<PlayerBehaviour>().PackEnergy(energyPack);

        Radar.RemoveRadarObject(this.gameObject);

        this.gameObject.SetActive(false);
        audioPlayer.PlaySFX(6);
    }
}
