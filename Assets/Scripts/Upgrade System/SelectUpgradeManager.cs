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
        playerCredits = PlayerState.GetCurrentPlayerState().GetCredits();

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

        PlayerState.GetCurrentPlayerState().SaveState();
    }

    private void GrantPlayerUpgrade()
    {
        PlayerState ps = PlayerState.GetCurrentPlayerState();

        if(currUpgrade.name.Equals("ArmorUpgradeLevel1"))
        {
            ps.AddPurchesedUpgraded(100);
        }
        else if(currUpgrade.name.Contains("ArmorUpgradeLeve2"))
        {
            ps.AddPurchesedUpgraded(200);
        }
        else if(currUpgrade.name.Contains("ArmorUpgradeLevel3"))
        {
            ps.AddPurchesedUpgraded(400);
        }
        else if (currUpgrade.name.Contains("SpeedUpgradeLevel1"))
        {
            ps.AddPurchesedUpgraded(10);
        }
        else if (currUpgrade.name.Contains("SpeedUpgradeLevel2"))
        {
            ps.AddPurchesedUpgraded(20);
        }
        else if (currUpgrade.name.Contains("SpeedUpgradeLevel3"))
        {
            ps.AddPurchesedUpgraded(40);
        }
    }
}
