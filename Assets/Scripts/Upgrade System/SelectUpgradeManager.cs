﻿using System;
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


    public void StartUI()
    {
        Debug.Log("does this fire");
        ps = PlayerState.GetCurrentPlayerState();

        InitializeUI();
        gameObject.SetActive(true);
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
        playerCreditsText.text = "Credits: " + playerCredits.ToString();
    }


    public void BuyButtonClicked()
    {
        if(playerCredits < currUpgrade.Cost)
        {
            Debug.Log("Player doesn't have enough credits to buy this thing. Maybe do a ui pop up or something idk");
            return;
        }

        if(UpgradeHasBeenPurchased() == true)
        {
            Debug.Log("Upgrade has already be bought");
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
            Apply(currUpgrade.upgradeLevel - 1);
        }
        else if (currUpgrade.upgradeType == UpgradeType.Track)
        {
            Apply(currUpgrade.upgradeLevel + 2);
        }
        else if (currUpgrade.upgradeType == UpgradeType.Gun)
        {
            Apply(currUpgrade.upgradeLevel + 5);
        }


    }

    private void Apply(int index)
    {
        char[] purchasedUpgrade = ps.GetPurchasedUpgrades().ToCharArray();


        purchasedUpgrade[index] = '1';

        String s = new string(purchasedUpgrade);

        ps.SetPurchasedUpgrades(s);

    }

    //This is pretty gross but it works so
    public void EquipUpgrade()
    {
        if(UpgradeHasBeenPurchased() == false)
        {
            Debug.Log("Upgrade needs to be purchased before it can be equipped");
            return;
        }


        string active = ps.GetActiveUpgrades();

        if (currUpgrade.upgradeType == UpgradeType.Armor)
        {
            active = currUpgrade.upgradeLevel + active.Substring(1, 2);
        }
        else if (currUpgrade.upgradeType == UpgradeType.Track)
        {
            active = active.Substring(0, 1) + currUpgrade.upgradeLevel + active.Substring(2, 1);
        }
        else if (currUpgrade.upgradeType == UpgradeType.Gun)
        {
            active = active.Substring(0, 2) + currUpgrade.upgradeLevel;
        }

        if(currUpgrade.upgradeType == UpgradeType.Armor)
        {
            ps.SetArmorUpgrade(currUpgrade);
        }
        else if (currUpgrade.upgradeType == UpgradeType.Track)
        {
            ps.SetTrackUpgrade(currUpgrade);
        }
        else
        {
            ps.SetGunUpgrade(currUpgrade);
        }

        ps.SetActiveUpgrades(active);
        ps.SaveState();

        DisplaySelectedUpgrades();
    }

    //This is mostly for testing, it'll probably be stripped out later
    public void ResetUpgrades()
    {
        ps.ResetUpgrades();
    }

    public void DisplaySelectedUpgrades()
    {
        DeselectAll();

        string selected = ps.GetActiveUpgrades();

        if(selected[0]=='1')
        {
            ArmorButtons[0].SelectUpgrade();
        }
        else if(selected[0]== '2')
        {
            ArmorButtons[1].SelectUpgrade();
        }
        else if (selected[0] == '3')
        {
            ArmorButtons[2].SelectUpgrade();
        }

        if (selected[1] == '1')
        {
            TreadButtons[0].SelectUpgrade();
        }
        else if (selected[1] == '2')
        {
            TreadButtons[1].SelectUpgrade();
        }
        else if (selected[1] == '3')
        {
            TreadButtons[2].SelectUpgrade();
        }

        if (selected[2] == '1')
        {
            GunButtons[0].SelectUpgrade();
        }
        else if (selected[2] == '2')
        {
            GunButtons[1].SelectUpgrade();
        }
        else if (selected[2] == '3')
        {
            GunButtons[2].SelectUpgrade();
        }
    }

    void DeselectAll()
    {
        for (int i = 0; i <= ArmorButtons.Length -1; i++)
        {
            ArmorButtons[i].DeSelectUpgrade();
            TreadButtons[i].DeSelectUpgrade();
            GunButtons[i].DeSelectUpgrade();
        }
    }

    public void DisplayUnlockedUpgrades()
    {
        string purchased = ps.GetPurchasedUpgrades();

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
                    GunButtons[i - 6].UnlockUpgrade();
                }
            }
        }
    }

    private bool UpgradeHasBeenPurchased()
    {
        char[] active = ps.GetPurchasedUpgrades().ToCharArray();
        int offset = 0;


        if(currUpgrade.upgradeType == UpgradeType.Armor)
        {
            offset = 0;
        }
        else if(currUpgrade.upgradeType == UpgradeType.Track)
        {
            offset = 3;
        }
        else
        {
            offset = 6;
        }

        if (active[currUpgrade.upgradeLevel - 1 + offset] == '1')
        {
            return true;
        }

        return false;
    }

    public static void DistributeUpGrades()
    {
        PlayerState ps = PlayerState.GetCurrentPlayerState();
        string active = ps.GetActiveUpgrades();

        // this is sloppy but it works
        if(active == null)
        {
            return;
        }

        UpgradeDatabase udb = Resources.Load<UpgradeDatabase>("UpgradeDatabase");

        int armorIndex = int.Parse(active.Substring(0, 1));
        int treadIndex = int.Parse(active.Substring(0, 1));
        int gunIndex = int.Parse(active.Substring(0, 1));

        if(armorIndex > 0)
        {
            ps.SetArmorUpgrade(udb.UpgradeList[armorIndex-1]);

        }

        if (treadIndex > 0)
        {
            ps.SetTrackUpgrade(udb.UpgradeList[treadIndex + 2]);

        }

        if(gunIndex > 0)
        {
            ps.SetGunUpgrade(udb.UpgradeList[gunIndex + 5]);

        }

    }

}
