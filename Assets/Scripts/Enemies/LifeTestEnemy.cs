using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTestEnemy : MonoBehaviour
{
    public int maxLife;
    public int life;

    void Start()
    {
        life = maxLife;
    } 

	public void TakeDamage(int amount)
    {
        life-= amount;
        Debug.Log("collision ok");

		if (life <= 0) this.gameObject.SetActive(false);
    }
}
