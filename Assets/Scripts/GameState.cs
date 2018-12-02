using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    
	public static bool GameIsPaused { get; set; }

    // The LevelStarted Evenet will fire whenever the level starts
    // The tanks will subscribe to these events to initialize their upgrades
    public delegate void OnLevelStart();
    public static event OnLevelStart LevelStarted;

    private static Difficulty CurrentDifficulty = Difficulty.Easy;

    
    // Call from difficulty selection screen to set difficulty
    public static void SetDifficulty(Difficulty d)
    {
        CurrentDifficulty = d;
    }

    // Get the current level difficulty selected by player
    public static Difficulty GetCurrentDifficultyObj()
    {
        return CurrentDifficulty;
    }

    // Get the current level difficulty selected by player
    public static string GetCurrentDifficultyStr()
    {
        return CurrentDifficulty.ToString();
    }
    
    // Get the current level difficulty selected by player
    public static int GetCurrentDifficultyInt()
    {
        return (int)CurrentDifficulty;
    }

    // Call when we need to fire the LevelStartedEvents
    public static void StartLevel()
    {
        Debug.Log(LevelStarted.Method);

        LevelStarted();
    }

    // Function for resetting gamplay and relevent values
    public static void RestartLevel()
    {
        /// todo: reset all the stuff that needs to be
        /// todo: whoever knows how to do this, please do it
        
        // Restart gameplay
        StartLevel();
    }

    // Function for pausing/freezing all gameplay actions
    public static void PauseLevel()
    {
        /// todo: if this needs modification
        GameIsPaused = true;
    }

    // Function for unpausing/unfreezing all gameplay actions
    public static void UnPauseLevel()
    {
        /// todo: if this needs modification
        GameIsPaused = false;
    }

    // Call on a gameplay end event
    public static void EndLevel(bool victory)
    {
        // freeze gameplay items/actions
        PauseLevel();

        // Capture relevant information (score, credits)
        // bool victory = false; /// todo: set vitory boolean from gamplay outcome
        int scoreEarned = GameObject.Find("TextScore").GetComponent<ScoreManager>().GetScore();

        // Calculate credits
        int multiplier = SetCreditMultiplier();
        int creditsEarned = scoreEarned / multiplier;

        // Display the victory/defeat panel
        GameObject.Find("CanvasMenus").GetComponent<VictoryDefeat>().DisplayOutcomePanel(victory, creditsEarned, scoreEarned);
    }

    // Set the credit earning rate based on level difficulty
    private static int SetCreditMultiplier()
    {
        int multiplier = 400;

        switch (GetCurrentDifficultyInt())
        {
            case 0:
                multiplier = 400;
                break;
            case 1:
                multiplier = 300;
                break;
            case 2:
                multiplier = 200;
                break;
            case 3:
                multiplier = 100;
                break;
            default:
                multiplier = 400;
                break;
        }

        return multiplier;
    }

}
