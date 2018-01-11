using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunMagazine : MonoBehaviour
{
    public LaserGun gun;
    public int gunMagazine;
    public Image icon;

    private void Start()
    {
        Radar.RegisterRadarObject(this.gameObject, icon);
    }
    void OnTriggerEnter(Collider other)
    {
        /*if (gun.ammo < gun.maxAmmo)
        {
            gun.ExtraAmmo(gunMagazine);

            Radar.RemoveRadarObject(this.gameObject);

            this.gameObject.SetActive(false);
        }*/
        gun.MagazineReload(gunMagazine);

        Radar.RemoveRadarObject(this.gameObject);

        this.gameObject.SetActive(false);
    }
}
