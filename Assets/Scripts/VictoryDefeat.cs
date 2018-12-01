﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryDefeat : MonoBehaviour {

    // Serverside script names referenced by WWWForms
    private readonly string URLGETLEADERSCORE = "action_requestlevelscore.php";

    // Colors for menu text
    private Color win = new Color32(24, 120, 5, 255);
    private Color loss = new Color32(219, 0, 0, 255);

    // To hold level high score info
    private Leaders thisLevelLeader;

    private int levelPlayed;
    private string difficultyPlayed;

    /***************************************
                Game Objects
     **************************************/
    // Panels
    public GameObject victoryDefeatUI;
    // Buttons
    public GameObject btnLevelSelect, btnMainMenu, btnRestart, btnUpgrades;
    // Text
    public Text txtTitle, txtLevel, txtDifficulty, txtScore, txtCredits, txtHighScore;

    
    // LevelSelect accessor
    private LevelSelect LS()
    {
        return GameObject.Find("CanvasMenus").GetComponent<LevelSelect>();
    }

    // MenuManager accessor
    private MenuManager MM()
    {
        return GameObject.Find("CanvasMenus").GetComponent<MenuManager>();
    }

    // PlayerState accessor
    private PlayerState PS()
    {
        return GameObject.Find("Manager").GetComponent<PlayerState>();
    }

    // Modify menu display attributes dased on victory or defeat outcome
    private void PanelLook(bool victory)
    {
        Text[] allTextArray = { txtTitle, txtLevel, txtDifficulty, txtScore, txtCredits, txtHighScore };
        if (victory)
        {
            txtTitle.text = "VICTORY!";
            foreach (Text txt in allTextArray) txt.color = win; 
        }
        else
        {
            txtTitle.text = "DEFEAT!";
            foreach (Text txt in allTextArray) txt.color = loss;
        }
    }

    // Call at match end to set and display the victory/defeat screen
    public void DisplayOutcomePanel(bool victory, int creditsEarned, int scoreEarned)
    {
        // Get match settings
        difficultyPlayed = GameState.GetCurrentDifficultyStr();
        levelPlayed = LS().GetCurrentLevel();

        // Call function to update player state and push to db
        PS().UpdateScore(levelPlayed, scoreEarned, creditsEarned, victory);

        // Call funtion to fetch high score for this level
        StartCoroutine("GetHighScore");
        
        // Call funtion to set panel look
        PanelLook(victory);

        // Update text objects for display
        txtLevel.text = "Map: " + levelPlayed.ToString();
        txtDifficulty.text = "Difficulty: " + difficultyPlayed;
        txtScore.text = "Your Score: " + scoreEarned.ToString();
        if (victory) txtCredits.text = "Credits Earned: " + creditsEarned.ToString();
        else txtCredits.text = "Credits Earned: 0";
        txtHighScore.text = "Map High Score: " + thisLevelLeader.scores + " (" + thisLevelLeader.users + ")";

        // Display the panel
        victoryDefeatUI.SetActive(true);
    }

    public void CloseOutcomePanel()
    {
        victoryDefeatUI.SetActive(false);
    }

    /***************************************
                Button Actions
     **************************************/
    public void LevelSelectButtonTapped()
    {
        // Display level selection as an overlay
        LS().DisplayLevelSelectPanel();
    }

    public void MainMenuButtonTapped()
    {
        // Close this panel and display main menu
        victoryDefeatUI.SetActive(false);
        MM().DisplayMainMenuPanel();
    }

    public void RestartLevelButtonTapped()
    {
        // Close this panel
        victoryDefeatUI.SetActive(false);

        // Call to restart game with current selections
        GameState.RestartLevel(); 
    }

    public void UpgradesButtonTapped()
    {
        // Display upgrades as an overlay
        MM().DisplayUpgradesPanel();
    }


    /***************************************
                Coroutines
     **************************************/
    private IEnumerator GetHighScore()
    {
        // Build the form for submission
        WWWForm form = new WWWForm();
        form.AddField("levelID", LS().GetCurrentLevel());

        WWW scoreReq = new WWW(PS().URL(URLGETLEADERSCORE), form);
        yield return scoreReq;

        // Check for successful web request
        if (string.IsNullOrEmpty(scoreReq.error))
        {
            // Convert response to JSON
            Leaders thisLevelLeader = JsonUtility.FromJson<Leaders>(scoreReq.text);

            // Check for failed update
            if (thisLevelLeader.query == false || thisLevelLeader.success == false)
            {
                // Log the error
                Debug.Log(thisLevelLeader.msg);
            }
        }
        else
        {
            // Log the connection error
            Debug.Log(scoreReq.error);
        }

    }
}
