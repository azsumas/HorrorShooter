using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFunctions : MonoBehaviour 
{
	
	public void SwitchOn () 
	{
		gameObject.SetActive (true);
		Debug.Log ("Lantern Switch ON");
	}
}
