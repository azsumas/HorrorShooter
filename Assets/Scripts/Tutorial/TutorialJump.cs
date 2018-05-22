using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialJump : MonoBehaviour
{
    public GameObject jumpText;

    void OnTriggerStay(Collider other)
    {
        jumpText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        jumpText.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
