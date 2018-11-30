using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectUpgradeManager : MonoBehaviour
{
    public Text upgradeTextDescription;
    public Text playerCreditsText;

    public UpgradeButton[] ArmorButtons;
    public UpgradeButton[] TreadButtons;
    public UpgradeButton[] GunButtons;

    private Upgrade currUpgrade;
    private int playerCredits;

    private PlayerState ps;

    private void OnEnable()
    {

        ps = PlayerState.GetCurrentPlayerState();

        InitializeUI();

    }

    public void InitializeUI()
    {
        playerCredits = ps.GetCredits();
        Debug.Log("Upgrade active");
        UpdateCreditDisplay();
        DisplaySelectedUpgrades();
        DisplayUnlockedUpgrades();
    }

    public void UpgradeButtonClicked(Upgrade upgradeClicked)
    {
        currUpgrade = upgradeClicked;

        UpdateDescription();
    }

    void UpdateDescription()
    {
        string dscript = "Cost: " + currUpgrade.Cost + "\n";
        dscript += currUpgrade.Description;
        upgradeTextDescription.text = dscript;
    }

    public void UpdateCreditDisplay()
    {
        playerCreditsText.text = playerCredits.ToString();
    }

    public void BuyButtonClicked()
    {
        if(playerCredits < currUpgrade.Cost)
        {
            Debug.Log("Player doesn't have enough credits to buy this thing. Maybe do a ui pop up or something idk");
            return;
        }

        GrantPlayerUpgrade();
        playerCredits -= currUpgrade.Cost;

        ps.SetCredits(playerCredits);

        ps.SaveState();

        InitializeUI();
    }

    private void GrantPlayerUpgrade()
    {        
        ps.AddPurchesedUpgraded(currUpgrade.masking);
    }

    //This is pretty gross but it works so
    public void EquipUpgrade()
    {
        string original = ps.GetActiveUpgrades().ToString();
        string replace = currUpgrade.masking.ToString();

        Debug.Log(original + " " + replace);

        int indexToreplace = replace.Length;
        string n = "";

        if (indexToreplace == 3)
        {
            n = replace.Substring(0,1) + original.Substring(1);
        }
        else if(indexToreplace == 2)
        {
            n =  original.Substring(0,1) + replace.Substring(0, 1) + original.Substring(2);

        }
        else if(indexToreplace == 1)
        {
            n = original.Substring(0, 2) + replace;
        }

        Debug.Log(n);

        ps.SetActiveUpgrades(int.Parse(n));
        ps.SaveState();

        InitializeUI();
    }

    //This is mostly for testing, it'll probably be stripped out later
    public void ResetUpgrades()
    {
        ps.ResetUpgrades();
    }

    public void DisplaySelectedUpgrades()
    {
        DeselectAll();

        char[] selected = ps.GetActiveUpgrades().ToString().ToCharArray();

        if(selected[0]==2)
        {
            ArmorButtons[0].SelectUpgrade();
        }
        else if(selected[0]==3)
        {
            ArmorButtons[1].SelectUpgrade();
        }
        else if (selected[0] == 4)
        {
            ArmorButtons[2].SelectUpgrade();
        }
    }

    void DeselectAll()
    {
        for (int i = 0; i < ArmorButtons.Length -1; i++)
        {
            ArmorButtons[i].DeSelectUpgrade();
            TreadButtons[i].DeSelectUpgrade();
            GunButtons[i].DeSelectUpgrade();
        }
    }

    public void DisplayUnlockedUpgrades()
    {

    }
}
