using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public Camera fpsCam;
    [Header("Settings gun")]
    public int damage = 10;
    public float range = 100f;
    public int maxAmmo;
    public int ammo;

    /*[Header("Effects")]
    public ParticleSystem flash;
    public GameObject impactEffect;*/

    // Use this for initialization
    void Start ()
    {
        ammo = maxAmmo;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (ammo >= 1)
            {
                //flash.Play();
                ammo--;
                Shot(); // Función del disparo
                Debug.Log("SHOT");
            }
        }
        //else flash.Stop();
    }

    public void ExtraAmmo(int magazine)
    {
        ammo += magazine;
    }

    void Shot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

			EnemyBehaviour target = hit.transform.GetComponent<EnemyBehaviour>(); //LifeTestEnemy - cambiar nombre de l'script para que el enemy reciba dañito!
            if (target != null)
            {
                target.SetDamage(damage);
            }
        }
        /*GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGo, 0.1f);*/

        Debug.Log("SHOT");
        Debug.DrawLine(fpsCam.transform.position, hit.point, Color.red, 4);
    }
}
