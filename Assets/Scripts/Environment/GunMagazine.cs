using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunMagazine : MonoBehaviour
{
    public LaserGun gun;
    public int gunMagazine;
    public Image icon;
    public GameObject managerScene;
    AudioPlayer audioPlayer;

    private void Start()
    {
        Radar.RegisterRadarObject(this.gameObject, icon);
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
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
        audioPlayer.PlaySFX(7);
    }
}
