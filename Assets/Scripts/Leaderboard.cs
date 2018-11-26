using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// User class to hold JSON results
[Serializable]
public class Leaders
{
    // Query response logic
    public bool query;
    public bool success;
    public string msg;

    // Leaderboard attributes
    public string users;
    public string scores;
}

public class Leaderboard : MonoBehaviour {

    // Serverside script names referenced by WWWForms
    private readonly string URLLEADERBOARD = "action_getleaders.php";

    /***************************************
                Game Objects
     **************************************/
    // Panels
    public GameObject leaderboardUI;
    // Buttons
    public GameObject btnClose;
    // Text
    public Text[] place = new Text[10];
    public Text[] userName = new Text[10];
    public Text[] score = new Text[10];

    // Activate and set the proper leaderboard objects
    public void BuildLeaderboard(Leaders l)
    {
        // Make query results itterable
        string[] lusers = l.users.Split(',');
        string[] lscore = l.scores.Split(',');

        // Display the leaderboard over other menu
        leaderboardUI.SetActive(true);

        // Loop through leaderboard text: activate & set if data exists
        for (int i = 0; i < lusers.Length; i++)
        {
            place[i].gameObject.SetActive(true);
            userName[i].gameObject.SetActive(true);
            userName[i].text = lusers[i];
            score[i].gameObject.SetActive(true);
            score[i].text = lscore[i];
        }
    }


    /***************************************
               Display Functions
    **************************************/
    // Display the leaderboard over other active panels
    public void DisplayLeaderboardPanel()
    {
        // Enable leaderboard panel
        StartCoroutine("GetLeaders");
    }


    /***************************************
                Button Actions
     **************************************/
    // Enable the leaderboard panel
    public void CloseButtonTapped()
    {
        // Diasble all the text objects before diabling the panel
        for (int i = 0; i < 10; i++)
        {
            place[i].gameObject.SetActive(false);
            userName[i].gameObject.SetActive(false);
            score[i].gameObject.SetActive(false);
        }

        // Disable leaderboard panel
        leaderboardUI.SetActive(false);
    }


    /***************************************
                Coroutines
     **************************************/
    private IEnumerator GetLeaders()
    {
        PlayerState ps = GameObject.Find("Manager").GetComponent<PlayerState>();
        // Build the form for submission
        WWW leaders = new WWW(ps.URL(URLLEADERBOARD));
        yield return leaders;

        // Check for successful web request
        if (string.IsNullOrEmpty(leaders.error))
        {
            // Convert response to JSON
            Leaders top10 = JsonUtility.FromJson<Leaders>(leaders.text);

            // Only proceed if credentials are valid and account is active
            if (top10.query == false || top10.success == false)
            {
                // Log the error
                Debug.Log(top10);
            }
            else
            {
                // Populate the leaderboard with results
                BuildLeaderboard(top10);
            }
        }
        else
        {
            // Log the connection error
            Debug.Log(leaders.error);
        }

    }
}
