using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserGun : MonoBehaviour
{
    public Camera fpsCam;
    public GunBar gunBar;
    private GameObject managerScene;
    [Header("Settings gun")]
    public int damage = 10;
    public float range = 100f;
    public float maxAmmo;
    public float ammo;
    public int magazine;
    public float fireRate = 10f;

    public Text magazineCount;

    [Header("Effects")]
    public ParticleSystem flash;
    public GameObject impactEffect;
    public GameObject enemyImpactEffect;
    public ParticleSystem smoke;
    public ParticleSystem smoke01;
    public GameObject impactParticle1;
    private float nextTimeToFire = 0f;

    private AudioPlayer audioPlayer;

    // Use this for initialization
    void Start ()
    {
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PauseManager.Instance.Pause)
        {
            if(ammo >= maxAmmo) ammo = maxAmmo;

            if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                if(ammo >= 1.0f)
                {
                    ammo--;
                    gunBar.AmmoShot(1);
                    Shot(); // Función del disparo
                    //Debug.Log("SHOT");
                    GameManager.instance.dataLogic.SetAmmo(ammo);
                }
                if(ammo <= 0.0f) ammo = 0.0f;
            }
            magazineCount.text = ammo + ("/") + magazine;
        }
    }

    public void MagazineReload(int reload)
    {
        magazine += reload;
    }

    public void Reload()
    {
        if(magazine >= 1 && ammo != maxAmmo)
        {
            audioPlayer.PlaySFX(4);
            ammo = maxAmmo;
            magazine -= 1;
        }
        gunBar.UpdateGunUI();
        GameManager.instance.dataLogic.SetAmmo(ammo);
    }

    void Shot()
    {
        flash.Play();
        smoke.Play();
        smoke01.Play();
        audioPlayer.PlaySFX(5);

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                EnemyBehaviour target = hit.transform.GetComponent<EnemyBehaviour>();
                if (target != null)
                {
                    target.SetDamage(damage);
                    GameObject enemyImpactGo = Instantiate(enemyImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(enemyImpactGo, 0.1f);
                }
            }
            else if (hit.collider.gameObject.CompareTag("EnemyRange"))
            {
                RangeEnemyBehaviour target = hit.transform.GetComponent<RangeEnemyBehaviour>();
                if (target != null)
                {
                    target.SetDamage(damage);
                }

            }
            else if (hit.collider.gameObject.CompareTag("EnemyDog"))
            {
                FasterEnemyBehaviour target = hit.transform.GetComponent<FasterEnemyBehaviour>();
                if (target != null)
                {
                    target.SetDamage(damage);
                }
            }
            else if (hit.collider.gameObject.CompareTag("Boss"))
            {
                BossBehaviour target = hit.transform.GetComponent<BossBehaviour>();
                if (target != null)
                {
                    damage = 2;
                    target.SetDamage(damage);
                }
            }
            else if (hit.collider.gameObject.CompareTag("BreakProp"))
            {
                Break target = hit.transform.GetComponent<Break>();
                if(target != null)
                {
                    target.SetDamage(damage);
                }
            }
            else if (hit.collider.gameObject.CompareTag("Critic"))
            {
                CriticDamage target = hit.transform.GetComponent<CriticDamage>();
                if (target != null)
                {
                    target.CriticDamages();
                }
            }
            else if (hit.collider.gameObject.CompareTag("Particle1"))
            {
                    GameObject impactParticle1GO = Instantiate(impactParticle1, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactParticle1GO, 10f);
            }
        }
        GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGo, 0.1f);
    }
}
