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
    Radar radar;
    RadarObject radObj;
    Collider m_collider;

    private void Start()
    {
        radar = GameObject.FindGameObjectWithTag("Radar").GetComponent<Radar>();

        icon = Instantiate(icon, radar.transform);
        icon.enabled = false;
        radObj = new RadarObject(this.gameObject, icon);

        radar.RegisterRadarObject(radObj);

        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
        m_collider = GetComponent<Collider>();

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

        radar.RemoveRadarObject(radObj);
        m_collider.enabled = !m_collider.enabled;

        audioPlayer.PlaySFX(6);
    }
}
