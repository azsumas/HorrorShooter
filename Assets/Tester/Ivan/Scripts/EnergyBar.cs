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
        player.energy = player.maxEnergy;
	}
	
	// Update is called once per frame
<<<<<<< HEAD
<<<<<<< HEAD
	public void RecivedDamage(float damage)
    {
        energy -= damage;
=======
	public void ReceivedDamage(float damage)
    {        
        player.energy -= damage;
>>>>>>> a8ce715006568505fa94c37b49e29d20369f63f9
=======
	public void ReceivedDamage(float damage)
    {        
        player.energy -= damage;
>>>>>>> a8ce715006568505fa94c37b49e29d20369f63f9
        UpdateEnergyUI();
    }

    private void UpdateEnergyUI()
    {
        energyBarImage.fillAmount = (1 / player.maxEnergy) * player.energy;
    }
}
