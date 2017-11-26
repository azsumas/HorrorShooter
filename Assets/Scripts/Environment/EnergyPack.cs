using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPack : MonoBehaviour
{
    public int energyPack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBehaviour>().energy < 100)
        {
            other.GetComponent<PlayerBehaviour>().RecoveryEnergy(energyPack);
            Destroy(this.gameObject);
            //this.gameObject.SetActive(false);
        }
    }
}
