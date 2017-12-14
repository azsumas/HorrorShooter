using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCondition : MonoBehaviour
{
	public void SetDeath ()
    {
        PlayerPrefs.SetInt("DeathOrNot", 1);
    }
}
