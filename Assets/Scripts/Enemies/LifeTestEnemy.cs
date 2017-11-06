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

    public void TakeDamage()
    {
        life--;
        Debug.Log("collision ok");
    }
}
