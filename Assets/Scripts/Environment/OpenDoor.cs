using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    bool open = false;
    [Header("Open speed and Position")]
    public GameObject text;
    public float openSpeed;
    public float maxOpen;
    public float posX;
    public float PosZ;

    // Use this for initialization
    void Update()
    {
        if(open == true)
        {
            posX += openSpeed;
            if (posX > maxOpen) posX = maxOpen;

            this.gameObject.transform.position = new Vector3(posX, 0, PosZ);
        }
        if (open == false)
        {
            posX -= openSpeed;
            if (posX <= 0) posX = 0;
            this.gameObject.transform.position = new Vector3 (posX, 0, PosZ);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) open = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            text.SetActive(true);
            open = false;
            text.SetActive(false);
        }
    }
}
