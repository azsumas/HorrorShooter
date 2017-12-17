using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{ 
    public Animator anim;

    void Start()
    {
        anim.GetComponent<Animator>();
    }

    public void OpenFinalDoor()
    {
        anim.SetTrigger("OpenDoors");
    }

    void PauseAnimationEvent()
    {
        anim.enabled = false;
    }
}
