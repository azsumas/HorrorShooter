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
    Radar radar;
    RadarObject radObj;
    Collider m_collider;

    private void Start()
    {
        radar = GameObject.FindGameObjectWithTag("Radar").GetComponent<Radar>();

        icon = Instantiate(icon, radar.transform);
        icon.enabled = false;
        radObj = new RadarObject(this.gameObject, icon);

        radar.RegisterRadarObject(radObj);

        managerScene = GameObject.FindWithTag("Manager");
        m_collider = GetComponent<Collider>();
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

        m_collider.enabled = !m_collider.enabled;
    }
}
