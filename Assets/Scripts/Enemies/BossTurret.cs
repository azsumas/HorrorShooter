using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurret : MonoBehaviour {

    public Transform target;

    [Header("Attributes")]
    public float range = 15f;
    public GameObject player;
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public float energy;

    public float fireRate = 5f;
    private float fireCountdown = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public FinalDoor finalDoor;

    public GameObject deathExp;


    // Use this for initialization
    void Start () {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
	}
	
    void UpdateTarget()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        float shortestDistance = Mathf.Infinity;

        float distanceToEnemy = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToEnemy < shortestDistance)
        {
            shortestDistance = distanceToEnemy;
        }
        if(shortestDistance <= range)
        {
            target = player.transform;
        }
    }
	// Update is called once per frame
	void Update () {
        if (target == null) return;
        //TargetLock;
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime* turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if(hit.collider)
            {
                Debug.Log("Dispara");
                if(fireCountdown <= 0f)
                {
                    if(hit.collider.gameObject.layer == 9)
                    {
                        Debug.Log("Detecta");
                        Shoot();
                        fireCountdown = 1f / fireRate;
                    }
                }

            }
        }
        fireCountdown -= Time.deltaTime;
	}
    
    void Shoot()
    {
        Debug.Log("Shoot");
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null)
        {
            bullet.Seek(target);
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    public void SetDamage(int hit)
    {
        energy -= hit;
        //energyBar.UpdateEnergyUI();

        if (energy <= 0)
        {
            //QUE HAGO CUANDO MUERE? PARTICULAS??? Desaparece???
            deathExp.SetActive(true);
            Death();
            return;
        }

    }

    private void Death()
    {
        finalDoor.OpenFinalDoor();
        Destroy(gameObject);
    }
}
