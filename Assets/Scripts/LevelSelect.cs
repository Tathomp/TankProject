using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{

    // State references, initialized in Start()
    MenuManager mm;
    PlayerState ps;

    private static int currentLevelSelected;

    /***************************************
                   Game Objects
    **************************************/
    // Buttons
    public GameObject[] btnLvls = new GameObject[4];
    // Panels
    public GameObject levelSelectUI;

    // LevelSelect accessor
    public static LevelSelect GetLevelSelectState()
    {
        return GameObject.Find("CanvasMenus").GetComponent<LevelSelect>();
    }

    void Start()
    {
        ps = PlayerState.GetCurrentPlayerState();
        mm = MenuManager.GetMenuManagerState();
    }


    // Use player state to configure buttons
    public void SetupButtons()
    {
        // Reset active level buttons
        SetButtonInteraction(ps.NUMBEROFLEVELS, false);
        
        // Unlock appropriate level buttons
        SetButtonInteraction(ps.GetMaxLevel(), true);
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

    // Get the current level selected by player
    public int GetCurrentLevel()
    {
        return currentLevelSelected;
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
        // Set the level scene to load using this levelSelevted attribute
        currentLevelSelected = levelSelected;

        Transform levelSpawn = GameObject.FindGameObjectWithTag("LevelSpawn").transform;

        if(levelSpawn.transform.childCount > 0)
        GameObject.Destroy(levelSpawn.GetChild(0).gameObject);

        GameObject go = Instantiate<GameObject>(
            GameObject.FindGameObjectWithTag("LevelDatabase").GetComponent<LevelDatabase>().Levels[levelSelected-1],
            levelSpawn
            );

        go.transform.parent = levelSpawn;

        // Display the difficulty selection panel as an overlay
        mm.difficultyUI.SetActive(true);
    }

}
