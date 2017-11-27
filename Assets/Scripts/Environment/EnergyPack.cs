using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPack : MonoBehaviour
{
    public int energyPack;
    public Image icon;

    private void Start()
    {
        Radar.RegisterRadarObject(this.gameObject, icon);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBehaviour>().energy < 100)
        {
            other.GetComponent<PlayerBehaviour>().RecoveryEnergy(energyPack);

            Radar.RemoveRadarObject(this.gameObject);

            this.gameObject.SetActive(false);
        }
    }
}
