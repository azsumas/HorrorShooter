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
    Radar radar;
    RadarObject radObj;

    private void Start()
    {
        radar = GameObject.FindGameObjectWithTag("Radar").GetComponent<Radar>();

        icon = Instantiate(icon, radar.transform);
        icon.enabled = false;
        radObj = new RadarObject(this.gameObject, icon);

        radar.RegisterRadarObject(radObj);

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
        radar.RemoveRadarObject(radObj);

        this.gameObject.SetActive(false);
        audioPlayer.PlaySFX(7);
    }
}
