using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesBar : MonoBehaviour
{
    public Image enemyBarImage;
    public EnemyBehaviour enemy;

    // Use this for initialization
    void Start()
    {
        UpdateEnergyUI();
    }

    public void ReceivedDamage(float damage)
    {
        enemy.energy -= damage;
        UpdateEnergyUI();
    }

    public void UpdateEnergyUI()
    {
        enemyBarImage.fillAmount = (1 / enemy.maxEnergy) * enemy.energy;
    }
}
