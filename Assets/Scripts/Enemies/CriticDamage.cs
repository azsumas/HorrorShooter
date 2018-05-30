using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticDamage : MonoBehaviour {
    public BossTurret bossScript;
    int hit = 20;
	// Use this for initialization

	
    public void CriticDamages()
    {
        bossScript.SetDamage(hit);
    }
}
