using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Image energyBarImage;
    public PlayerBehaviour player;

	// Use this for initialization
	void Start ()
    {
        UpdateEnergyUI();
    }

    public void ReceivedDamage(float damage)
    {
        player.energy -= damage;
        UpdateEnergyUI();
    }

    private void UpdateEnergyUI()
    {
        energyBarImage.fillAmount = (1 / player.maxEnergy) * player.energy;
    }
}
