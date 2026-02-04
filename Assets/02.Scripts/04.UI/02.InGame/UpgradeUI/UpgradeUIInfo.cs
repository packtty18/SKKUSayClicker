using Sirenix.OdinInspector;
using System;
using UnityEngine;


[Serializable]
public struct UpgradeUIInfo
{
    public string Name;
    [PreviewField]
    public Sprite Icon;
    [TextArea]
    public string Description;

    public UpgradeLevelInfo[] LevelsData;
}
