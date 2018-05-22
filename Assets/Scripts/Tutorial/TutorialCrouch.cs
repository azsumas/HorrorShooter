using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCrouch : MonoBehaviour
{
    public GameObject crouchText;

    void OnTriggerStay(Collider other)
    {
        crouchText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        crouchText.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
