using UnityEngine;

public interface IReadOnlyUpgrade
{
    float Value { get; }

    UpgradeSpecData Spec { get; }
    int Level { get; }
    public SCurrency Cost { get; }
    bool IsMaxLevel { get; }
    bool CanLevelUp();
}