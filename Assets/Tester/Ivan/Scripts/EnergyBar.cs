using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Image energyBarImage;
    public float maxEnergy;
    public float energy;

	// Use this for initialization
	void Start ()
    {

        energy = maxEnergy;
	}
	
	// Update is called once per frame
	public void RecivedDamage(float damage)
    {
        
        energy -= damage;
        UpdateEnergyUI();
    }

    private void UpdateEnergyUI()
    {
        energyBarImage.fillAmount = (1 / maxEnergy) * energy;
    }
}
