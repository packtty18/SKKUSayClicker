using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class UpgradeSpecData
{
    public EUpgradeType Type;
    public int MaxLevel;
    public float BaseCost;
    public float BaseValue;
    public float CostMultiplier;
    public float ValueMultiplier;

    //UI Only
    public string Name;
    [TextArea]
    public string Description;
    [PreviewField]
    public Sprite Icon;
}
