using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private static int score;

    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
        text.text = "" + score;
	}

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int points)
    {
        score = points;
    }

    public void IncreaseScore(int points)
    {
        score += points;
    }

    public void DecreaseScore(int points)
    {
        score -= points;
    }
}
