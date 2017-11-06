using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public Camera fpsCam;
    [Header("Settings gun")]
    public float damage = 10f;
    public float range = 100f;
    public int maxAmmo;
    public int ammo;


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
                ammo--;
                Shot(); // Función del disparo
            }
        }
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

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
        Debug.Log("SHOT");
        Debug.DrawLine(fpsCam.transform.position, hit.point, Color.red, 4);
    }
}
