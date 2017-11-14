using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunBar : MonoBehaviour
{
    public Image gunBarImage;
    public LaserGun gun;

    // Use this for initialization
    void Start()
    {
        UpdateGunUI();
    }

    public void AmmoShot(int shot)
    {
        gun.ammo -= shot;
        UpdateGunUI();
    }

    public void UpdateGunUI()
    {
        gunBarImage.fillAmount = (1 / gun.maxAmmo) * gun.ammo;
    }
}
