using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade/NewUpgrade")]
public class Upgrade : ScriptableObject
{
    public string Upgradename;
    public string Description;
    public int Cost;
    public int masking;

    public List<UpgradeEffect> UpgradeEffects;
    public UpgradeType upgradeType;

    public void AddEffect(Tank tank)
    {
        foreach (UpgradeEffect upgradeEffect in UpgradeEffects)
        {
            upgradeEffect.AddEffect(tank);
        }
    }

    public void RemoveEffect(Tank tank)
    {
        foreach (UpgradeEffect upgradeEffect in UpgradeEffects)
        {
            upgradeEffect.RemoveEffect(tank);
        }
    }
}

public enum UpgradeType
{
    Gun,
    Armor,
    Track
   
}




