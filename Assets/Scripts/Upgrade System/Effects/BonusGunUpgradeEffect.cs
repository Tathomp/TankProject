using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Upgrade Effect", menuName = "Upgrade/GunUpgrade")]
public class BonusGunUpgradeEffect : UpgradeEffect
{
    public int NumberOfShots;

    public override void AddEffect(Tank tank)
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveEffect(Tank tank)
    {
        throw new System.NotImplementedException();
    }
}
