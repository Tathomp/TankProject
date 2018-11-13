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
        GunUpgrade.AddEffect(this);
        ArmorUpgrade.AddEffect(this);
        TrackUpgrade.AddEffect(this);
    }

    public void RemoveUpgrades()
    {
        GunUpgrade.RemoveEffect(this);
        ArmorUpgrade.RemoveEffect(this);
        TrackUpgrade.RemoveEffect(this);
    }
}
