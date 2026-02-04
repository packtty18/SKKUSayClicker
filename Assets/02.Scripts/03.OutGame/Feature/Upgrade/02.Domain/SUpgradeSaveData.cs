using Firebase.Firestore;
using System;
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

[Serializable]
[FirestoreData]
public class FirebaseUpgradeSaveData
{
    [FirestoreProperty]
    public int[] Level { get; set; }

    public FirebaseUpgradeSaveData()
    {

    }
    public FirebaseUpgradeSaveData(SUpgradeSaveData data)
    {
        Level = data.Level;
    }
}
