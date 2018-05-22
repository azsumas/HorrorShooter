using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTutorial : MonoBehaviour
{
    public GameObject aimText;

    void OnTriggerStay(Collider other)
    {
        aimText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        aimText.SetActive(false);
        this.gameObject.SetActive(false);
    }
}