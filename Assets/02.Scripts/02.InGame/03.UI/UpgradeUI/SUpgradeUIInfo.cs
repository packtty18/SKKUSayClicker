using Sirenix.OdinInspector;
using System;
using UnityEngine;


[Serializable]
public struct SUpgradeUIInfo
{
    public string Name;
    [PreviewField]
    public Sprite Icon;
    [TextArea]
    public string Description;

    public SLevelInfo[] LevelsData;
}

[Serializable]
public struct SLevelInfo
{
    public int Level;
    public ECurrencyType NextCostType;
    public float NextCost;

    public void Default()
    {
        Level = 0;
        NextCostType = ECurrencyType.Money;
        NextCost = 0;
    }
}
