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

    private Upgrade currUpgrade;
    private int playerCredits;

    private void OnEnable()
    {
        InitializeUI();
    }

    public void InitializeUI()
    {
        playerCredits = PlayerState.GetCurrentPlayerState().GetCredits();
        Debug.Log("Upgrade active");
        UpdateCreditDisplay();
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

        PlayerState.GetCurrentPlayerState().SetCredits(playerCredits);

        PlayerState.GetCurrentPlayerState().SaveState();

        InitializeUI();
    }

    private void GrantPlayerUpgrade()
    {
        PlayerState ps = PlayerState.GetCurrentPlayerState();
 
        ps.AddPurchesedUpgraded(currUpgrade.masking);
    }

    //This is pretty gross but it works so
    public void EquipUpgrade()
    {
        string original = PlayerState.GetCurrentPlayerState().GetEquipped().ToString();
        string replace = currUpgrade.masking.ToString();

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

        PlayerState.GetCurrentPlayerState().SetActiveUpgrades(int.Parse(n));
        PlayerState.GetCurrentPlayerState().SaveState();
    }

    //This is mostly for testing, it'll probably be stripped out later
    public void ResetUpgrades()
    {
        PlayerState.GetCurrentPlayerState().ResetUpgrades();
    }
}
