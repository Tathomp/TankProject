using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainerManager : MonoBehaviour {

    List<GameObject> EnemyCopyContainer;

    public void InitCopy()
    {
        EnemyCopyContainer = new List<GameObject>();

        //Copying all the Enemies into an array so that we can reset them easily
        for (int i = 0; i < transform.childCount; i++)
        {
            EnemyCopyContainer.Add( GameObject.Instantiate(transform.GetChild(i).gameObject));
        }
    }

	// I'll have to remember to change it so this runs after all other scripts
	void Start () {
		
	}
	
}
