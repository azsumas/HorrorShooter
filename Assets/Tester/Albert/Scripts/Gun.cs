using UnityEngine;

public class Gun : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;


    public Camera fpsCam;
    public ParticleSystem flash;
    public GameObject impactEffect;
   // public ParticleSystem laserEffect;


	// Update is called once per frame
	void Update () {

        

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
	}

    void Shoot()
    {
        flash.Play();
        
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 0.05f);
            //ParticleSystem laserGo = Instantiate(laserEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(laserGo, 0.05f);
        }
    }

}
