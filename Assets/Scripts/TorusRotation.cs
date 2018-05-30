using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusRotation : MonoBehaviour {

    public float angularV = 1.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * angularV * Time.deltaTime, Space.Self);

    }
}