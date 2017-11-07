using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMagazine : MonoBehaviour
{
    public LaserGun gun;
    public int gunMagazine;

    void OnTriggerEnter(Collider other)
    {
        if (gun.ammo < gun.maxAmmo)
        {
            gun.ExtraAmmo(gunMagazine);
            this.gameObject.SetActive(false);
        }
    }
}
