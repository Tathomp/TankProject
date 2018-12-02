using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreControl : MonoBehaviour {
    Logger scoreLogger;

    public static int playerScore;
	// Use this for initialization
	void Start () {
        

        playerScore = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void increment()
    {
        scoreLogger = new Logger(Debug.unityLogger.logHandler);
        playerScore += 1;
        scoreLogger.Log("player score : " + playerScore);
    }
}
