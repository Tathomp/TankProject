using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// This will be the base class for the player and ai controlled tanks
/// Currently I'm just going to have the upgrade components, but we can use it to hold other things
/// like movement speed, health, etc
/// 

abstract public class Tank : MonoBehaviour {

    public int MaxHealth;
    public int CurrentHealth;

    public Upgrade GunUpgrade;
    public Upgrade ArmorUpgrade;
    public Upgrade TrackUpgrade;



    public void ApplyUpgrades()
    {
        ApplyUpgrade(GunUpgrade);
        ApplyUpgrade(ArmorUpgrade);
        ApplyUpgrade(TrackUpgrade);

    }

    void ApplyUpgrade(Upgrade upgrade)
    {
        if(upgrade != null)
        {
            upgrade.AddEffect(this);
        }
    }

    public void RemoveUpgrades()
    {
        RemoveUpgrade(GunUpgrade);
        RemoveUpgrade(ArmorUpgrade);
        RemoveUpgrade(TrackUpgrade);
    }

    private void RemoveUpgrade(Upgrade upgrade)
    {
        if(upgrade != null)
        {
            upgrade.RemoveEffect(this);
        }
    }

    public void ResetTank()
    {
        CurrentHealth = MaxHealth;

        RemoveUpgrades();

        gameObject.SetActive(true);
    }
}
