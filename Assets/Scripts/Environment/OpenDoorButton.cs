﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorButton : MonoBehaviour
{
    public Animator anim;
    public GameObject text;
    float time;
    public float maxTime;
    bool canOpenDoor;
    public bool opening;

    void Start()
    {
        anim.GetComponent<Animator>();
        canOpenDoor = false;
    }

    void Update()
    {
        if (canOpenDoor == true)
        {
            time += Time.deltaTime;
            if (time >= maxTime)
            {
                anim.enabled = true;
                canOpenDoor = false;
                time = 0;
                return;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        text.SetActive(true);

        if (Input.GetButtonDown("ActiveDoor"))//if (opening == true) // Cambiamos el bool desde el input manager. 
        {
            anim.SetTrigger("OpenDoors");
            canOpenDoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        text.SetActive(false);
    }

    void PauseAnimationEvent()
    {
        anim.enabled = false;
    }
}
