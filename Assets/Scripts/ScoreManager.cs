using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private static int score;

    // UI Text Object
    public Text text;

	// Use this for initialization
	void Start () {
        text.text = "0";
        SetScore(0);

        GameState.LevelStarted += ResetScore;
	}
	
	// Update is called once per frame
	void Update ()
    {
        text.text = score.ToString();
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

    public void ResetScore()
    {
        SetScore(0);
    }
}
