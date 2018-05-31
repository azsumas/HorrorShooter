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

    public Text ammoCount;
    public Text magazineCount;
    public Text reloadAmmo;

    [Header("Effects")]
    public ParticleSystem flash;
    public GameObject impactEffect;
    public GameObject enemyImpactEffect;
    public ParticleSystem smoke;
    public ParticleSystem smoke01;
    public GameObject impactParticle1;
    public GameObject impactParticle2;
    public GameObject impactParticleExt;
    public GameObject impactParticleBoss;

    private float nextTimeToFire = 0f;
    public GameObject lightFlash;
    private AudioPlayer audioPlayer;
    bool playSound = false;

    [Header("Animation")]
    public Animator animGun;
    public PlayerBehaviour playerScript;


    // Use this for initialization
    void Start ()
    {
        managerScene = GameObject.FindWithTag("Manager");
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        lightFlash.SetActive(false);
        if (!PauseManager.Instance.Pause)
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
            else animGun.SetBool("isShooting", false);

            ammoCount.text = ammo + ("/");
            magazineCount.text = (" ") + magazine;

            if (magazine <= 0)
            {
                magazineCount.color = new Vector4(220, 0, 0, 1);
            }
            else magazineCount.color = new Vector4(224, 245, 236, 1);

            if (ammo <= 5) ammoCount.color = new Vector4(220, 0, 0, 1);
            else ammoCount.color = new Vector4(224, 245, 236, 1);

            if (ammo <= 5 && magazine >= 1)
            {
                reloadAmmo.enabled = true;
                PlaySound();
            }
            else
            {
                reloadAmmo.enabled = false;
                playSound = false;
            }

                if (playerScript.moveDirection.x == 0 || playerScript.moveDirection.z == 0)
            {
                animGun.SetBool("isWalking", false);
            }
            else if(playerScript.moveDirection.x != 0 || playerScript.moveDirection.z != 0)
            {
                animGun.SetBool("isWalking", true);
            }

            if (playerScript.aiming == true)
            {
                animGun.SetBool("isAiming", true);
            }
            else animGun.SetBool("isAiming", false);
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

    public void PlaySound()
    {
        if (playSound == true) return;
        audioPlayer.PlaySFX(24);
        playSound = true;
    }

    void Shot()
    {
        flash.Play();
        smoke.Play();
        smoke01.Play();
        lightFlash.SetActive(true);
        audioPlayer.PlaySFX(5);
        animGun.SetBool("isShooting", true);
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
                    Destroy(enemyImpactGo, 0.5f);
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
                GameObject impactParticleBossGO = Instantiate(impactParticleBoss, hit.point, Quaternion.LookRotation(hit.normal));
                BossTurret target = hit.transform.GetComponent<BossTurret>();

                if (target != null)
                {
                    target.SetDamage(damage);
                }

                Destroy(impactParticleBossGO, 10f);
            }
            else if (hit.collider.gameObject.CompareTag("BreakProp"))
            {
                Break target = hit.transform.GetComponent<Break>();

                if(target != null)
                {
                    target.SetDamage(damage);
                }
            }
            else if (hit.collider.gameObject.CompareTag("extintor"))
            {
                GameObject impactParticleExtGO = Instantiate(impactParticleExt, hit.point, Quaternion.LookRotation(hit.normal));
                extintor target = hit.transform.GetComponent<extintor>();

                if (target != null)
                {
                    target.SetDamage(damage);
                }

                Destroy(impactParticleExtGO, 10f);
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
            else if (hit.collider.gameObject.CompareTag("Particle2"))
            {
                GameObject impactParticle2GO = Instantiate(impactParticle2, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactParticle2GO, 10f);
            }
        }
        GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGo, 0.1f);
    }
}
