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

    public void UpdateEnergyUI()
    {
        energyBarImage.fillAmount = (1 / player.maxEnergy) * player.energy;
    }

    public void LowEnergy()
    {
        energyBarImage.color = Color.red;
    }

    public void HighEnergy()
    {
        energyBarImage.color = new Vector4(224, 245, 236, 1);
    }
}
