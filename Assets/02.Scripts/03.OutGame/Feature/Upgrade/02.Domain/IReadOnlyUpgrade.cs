using UnityEngine;

public interface IReadOnlyUpgrade
{
    UpgradeSpecData Spec { get; }
    int Level { get; }
    public SCurrency Cost { get; }
    bool IsMaxLevel { get; }
    bool CanLevelUp();
}