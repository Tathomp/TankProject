using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHUDManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameState.LevelStarted += InitHUD;
	}
	
	public void InitHUD()
    {
        gameObject.SetActive(true);
    }
}
