using UnityEngine;

public struct SUpgradeSaveData
{
    public int[] Level;

    // 재화 기본값
    public static SUpgradeSaveData Default => new SUpgradeSaveData()
    {
        Level = new int[(int)EUpgradeType.Count]
    };
}
