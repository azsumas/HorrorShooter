using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public Camera fpsCam;
    public GameObject laser;
    [Header("Settings gun")]
    public float damage = 10f;
    public float range = 100f;
    int startShot; // Valor para la velocidad en la que sale el "laser"

	// Use this for initialization
	void Start ()
    {
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            startShot += 3;
            if (startShot > 10) startShot = 10; // Distancia máxima a la que se va a dibujar el "laser"
            laser.GetComponent<LineRenderer>().SetPosition(0, Vector3.forward*startShot); // Coger el componente de LineRenderer y darle la posición de dibujado
            Shot(); // Función del disparo
        }
        else
        {
            startShot = 0;
            laser.SetActive(false);
            Debug.Log("NO SHOT!");
        }
    }

    void Shot()
    {
        laser.SetActive(true);

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        Debug.Log("SHOT");
        Debug.DrawLine(fpsCam.transform.position, hit.point, Color.red, 4);
    }
}
