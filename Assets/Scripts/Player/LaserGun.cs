using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public Camera fpsCam;
    public GunBar gunBar;
    [Header("Settings gun")]
    public int damage = 10;
    public float range = 100f;
    public float maxAmmo;
    public float ammo;
    public float fireRate = 10f;

    [Header("Effects")]
    public ParticleSystem flash;
    public GameObject impactEffect;
    public ParticleSystem smoke;
    public ParticleSystem smoke01;

    private float nextTimeToFire = 0f;

    // Use this for initialization
    void Start ()
    {
        //ammo = maxAmmo;
	}

    // Update is called once per frame
    void Update()
    {
        if (ammo >= maxAmmo) ammo = maxAmmo;

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            if (ammo >= 1.0f)
            {
                ammo--;
                gunBar.AmmoShot(1);
                Shot(); // Función del disparo
                Debug.Log("SHOT");
            }
        }

    }

    public void ExtraAmmo(int magazine)
    {
        ammo += magazine;
        gunBar.UpdateGunUI();
    }

    void Shot()
    {
        flash.Play();
        smoke.Play();
        smoke01.Play();

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
        GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGo, 0.1f);

        Debug.Log("SHOT");
        Debug.DrawLine(fpsCam.transform.position, hit.point, Color.red, 4);
    }
}
