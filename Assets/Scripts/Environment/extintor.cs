using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extintor : MonoBehaviour {

    public float energy;

    public GameObject smokeExt;
    public Collider col;
    // Use this for initialization
    void Start()
    {

    }

    public void SetDamage(int hit)
    {
        energy -= hit;
        smokeExt.SetActive(true);
        //energyBar.UpdateEnergyUI();

        if (energy <= 0)
        {
            Debug.Log("Entra");
            Die();

        }
    }
    void Die()
    {
        col.enabled = false;
        return;
    }
}
