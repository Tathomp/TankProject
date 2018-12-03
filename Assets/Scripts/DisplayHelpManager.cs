using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHelpManager : MonoBehaviour {

    private bool prevPauseState;

    public void DisplayHelpPanel()
    {
        prevPauseState = GameState.GameIsPaused;

        GameState.PauseLevel();
        this.gameObject.SetActive(true);
    }

    public void ExitHelpMenu()
    {
        this.gameObject.SetActive(false);

        if(prevPauseState == false)
        {
            GameState.UnPauseLevel();
        }
    }

    public void ToggleMenu()
    {
        if(this.gameObject.activeInHierarchy)
        {
            ExitHelpMenu();
        }
        else
        {
            DisplayHelpPanel();
        }
    }
}
