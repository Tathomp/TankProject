using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectUpgradeManager : MonoBehaviour
{ 

    // assigned through the editor
    public UpgradeButton buttonPrefab;
    public Transform buttoncontainer;

    // properties we use 
    List<UpgradeButton> buttons;


	// Use this for initialization
	void Start () {
        InitUpgradeDisplay();
	}
	



    public void InitUpgradeDisplay()
    {
        List<Upgrade> upgrades = Resources.Load<UpgradeDatabase>("UpgradeDatabase").UpgradeList;

        buttons = new List<UpgradeButton>();

        foreach (Upgrade upgrade in upgrades)
        {
            SpawnButton(upgrade);
        }
    }

    private void SpawnButton(Upgrade upgrade)
    {
        UpgradeButton btn = Instantiate<UpgradeButton>(buttonPrefab, buttoncontainer);
        btn.InitButton(upgrade);
        buttons.Add(btn);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }


    /// <summary>
    /// We do this because listeners can presist between seens causing a memory leak if this 
    /// scene is load and unload a lot of times in one session
    /// 
    /// Also we need to destory the buttons and repopulate them anytime the scene is load
    /// 
    /// We could just set it up manully in the editor but dynamically creating buttons is more scallable
    /// as new upgrades are added/removed/changed
    /// </summary>
    /// 

    private void CleanUpButtons()
    {
        for (int i = buttons.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(buttons[i]);
            GameObject.Destroy(buttons[i].gameObject);
        }

        buttons = new List<UpgradeButton>();
    }

    private void OnDisable()
    {
        CleanUpButtons();
    }
}
