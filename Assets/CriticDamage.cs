using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticDamage : MonoBehaviour {
    public BossBehaviour bossScript;
    int hit = 20;
	// Use this for initialization
	void Start () {
        bossScript = GetComponent<BossBehaviour>();
	}
	
	// Update is called once per frame
	void Update () { }

    public void CriticDamages()
    {
        bossScript.SetDamage(hit);
    }
}
