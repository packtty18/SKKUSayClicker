using Firebase.Firestore;
using System;
using UnityEngine;

public struct UpgradeSaveData
{
    public int[] Level;

    // 재화 기본값
    public static UpgradeSaveData Default => new UpgradeSaveData()
    {
        Level = new int[(int)EUpgradeType.Count]
    };
}