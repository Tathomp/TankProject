using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "Upgrade/UpgradeDatabase")]
public class UpgradeDatabase : ScriptableObject
{
    public List<Upgrade> UpgradeList = new List<Upgrade>();
}
