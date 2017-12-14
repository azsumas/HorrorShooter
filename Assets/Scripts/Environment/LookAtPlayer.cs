using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {
    public Transform targetTransform;

    // Use this for initialization
    void Start () {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(targetTransform);
    }
}
