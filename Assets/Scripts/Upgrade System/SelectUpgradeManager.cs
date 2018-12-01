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

        if(currUpgrade.upgradeType == UpgradeType.Armor)
        {
            Apply(currUpgrade.upgradeLevel);
        }
        else if (currUpgrade.upgradeType == UpgradeType.Track)
        {
            Apply(currUpgrade.upgradeLevel + 3);
        }
        else if (currUpgrade.upgradeType == UpgradeType.Gun)
        {
            Apply(currUpgrade.upgradeLevel + 6);
        }


    }

    private void Apply(int index)
    {
        char[] purchasedUpgrade = ps.GetPurchasedUpgrades().ToCharArray();


        purchasedUpgrade[index] = '1';

        ps.SetPurchasedUpgrades(purchasedUpgrade.ToString());

    }

    //This is pretty gross but it works so
    public void EquipUpgrade()
    {
        char[] active = ps.GetActiveUpgrades().ToCharArray();

        if (currUpgrade.upgradeType == UpgradeType.Armor)
        {
            active[0] = (Char)currUpgrade.upgradeLevel;
        }
        else if (currUpgrade.upgradeType == UpgradeType.Track)
        {
            active[1] = (Char)currUpgrade.upgradeLevel;
        }
        else if (currUpgrade.upgradeType == UpgradeType.Gun)
        {
            active[2] = (Char)currUpgrade.upgradeLevel;
        }


        ps.SetActiveUpgrades(active.ToString());
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
        char[] purchased = ps.GetPurchasedUpgrades().ToString().ToCharArray();

         //Check Armor Upgrades
        for (int i = 0; i < purchased.Length; i++)
        {
            if(purchased[i] == '1')
            {
                if(i < 3)
                {
                    ArmorButtons[i].UnlockUpgrade();
                }
                else if(i<6)
                {
                    TreadButtons[i - 3].UnlockUpgrade();

                }
                else
                {
                    GunButtons[i - 3].UnlockUpgrade();
                }
            }
        }
    }

    void ToogleOffUnlockImage(int indexTounlock, UpgradeButton[] upgradeClass)
    {
        upgradeClass[0].UnlockUpgrade();
    }
}
