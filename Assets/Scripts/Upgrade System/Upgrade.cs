using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade/NewUpgrade")]
public class Upgrade : ScriptableObject
{
    public List<UpgradeEffect> UpgradeEffects;
    public UpgradeType upgradeType;

    public void AddEffect()
    {
        foreach (UpgradeEffect upgradeEffect in UpgradeEffects)
        {
            upgradeEffect.AddEffect();
        }
    }

    public void RemoveEffect()
    {
        foreach (UpgradeEffect upgradeEffect in UpgradeEffects)
        {
            upgradeEffect.RemoveEffect();
        }
    }
}

public enum UpgradeType
{
    Gun,
    Armor
}




