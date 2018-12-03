using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDMenuButtonManager : MonoBehaviour {

	
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.GetComponent<Button>().onClick.Invoke();
        }
	}
}
