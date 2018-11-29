using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

    /***************************************
                   Game Objects
    **************************************/
    // Buttons
    public GameObject[] btnLvls = new GameObject[4];
    // Panels
    public GameObject levelSelectUI;


    // MenuManager accessor
    public MenuManager MM()
    {
        return GameObject.Find("CanvasMenus").GetComponent<MenuManager>();
    }

    // PlayerState accessor
    public PlayerState PS()
    {
        return GameObject.Find("Manager").GetComponent<PlayerState>();
    }

    // Use player state to configure buttons
    public void SetupButtons()
    {
        // Reset active level buttons
        SetButtonInteraction(4, false);
        
        // Unlock appropriate level buttons
        SetButtonInteraction(PS().GetMaxLevel(), true);
    }

    // Sets button interatable value
    public void SetButtonInteraction(int lvl, bool stat)
    {
        // Level 1 should always be active, start at index 1
        for (int i = 1; i < lvl; i++)
        {
            btnLvls[i].GetComponent<Button>().interactable = stat;
        }
    }


    /***************************************
                Display Functions
    **************************************/

    // Display the level select panel as an overlay
    // Overlay: meaning calling menu is still active underneath
    // May be called from multiple menus
    public void DisplayLevelSelectPanel()
    {
        levelSelectUI.SetActive(true);
        SetupButtons();
    }


    /***************************************
                Button Actions
     **************************************/
    // Enable the level selecter panel
    public void BackButtonTapped()
    {
        levelSelectUI.SetActive(false);
    }

    // Set level selected and display difficulty menu
    public void LevelButtonTapped(int levelSelected)
    {
        /// Set the level scene to load using this levelSelevted attribute
        /// Ex: levelID = levelSelected;

        // Display the difficulty selection panel as an overlay
        MM().difficultyUI.SetActive(true);
    }

}
