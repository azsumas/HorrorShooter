using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMove : MonoBehaviour {

    private NavMeshAgent theAgent;
    public Transform target;

	// Use this for initialization
	void Start () {
        theAgent = GetComponent<NavMeshAgent>();

	}
	
	// Update is called once per frame
	void Update () {
        theAgent.SetDestination(target.position);
	}
}
