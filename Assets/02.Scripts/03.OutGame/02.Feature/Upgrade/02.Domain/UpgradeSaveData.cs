using Firebase.Firestore;
using System;
using UnityEngine;

public struct UpgradeSaveData
{
    public int[] Levels;
    public Timestamp LastSavedAt;

    // 재화 기본값
    public static UpgradeSaveData Default => new UpgradeSaveData()
    {
        Levels = new int[(int)EUpgradeType.Count],
        LastSavedAt = Timestamp.FromDateTime(DateTime.MinValue),
    };

    public static UpgradeSaveData FromFirebase(FirebaseUpgradeSaveData firebaseData)
    {
        if (firebaseData == null)
        {
            return Default;
        }

        return new UpgradeSaveData
        {
            Levels = firebaseData.Levels,
            LastSavedAt = firebaseData.LastSavedAt,
        };
    }
}