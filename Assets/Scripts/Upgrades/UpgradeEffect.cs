using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Effect", menuName = "Upgrade/TestEffect")]
public class UpgradeEffect : ScriptableObject
{

	// Use this for initialization
	public void Test () {
        Debug.Log("Does this even fire man");
	}
	
}
