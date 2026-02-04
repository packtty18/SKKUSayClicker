using UnityEngine;

public interface IReadOnlyUpgrade
{
    float Value { get; }

    UpgradeSpecData Spec { get; }
    int Level { get; }
    public Currency Cost { get; }
    bool IsMaxLevel { get; }
    bool CanLevelUp();
}