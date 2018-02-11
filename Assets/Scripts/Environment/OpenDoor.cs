using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("OpenDoor");
    }

    private void OnTriggerExit(Collider other)
    {
        anim.enabled = true;
    }

    void PauseAnimationEvent()
    {
        anim.enabled = false;
    }
}
