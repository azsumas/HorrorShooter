using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{

    public float energy;

    public GameObject normal;
    public GameObject roto;

    // Use this for initialization
    void Start()
    {

    }

    public void TakeDamage(int hit)
    {
        energy -= hit;
        //energyBar.UpdateEnergyUI();

        if (energy <= 0)
        {
            Debug.Log("Entra");
            Die();
            
            return;
        }
    }
    void Die()
    {
        normal.SetActive(false);
        roto.SetActive(true);
        Debug.Log("Cambio");
    }
}
