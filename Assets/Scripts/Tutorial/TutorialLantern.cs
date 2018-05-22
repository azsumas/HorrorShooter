using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLantern : MonoBehaviour
{
    public GameObject lanternText;

    void OnTriggerStay(Collider other)
    {
        lanternText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        lanternText.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
