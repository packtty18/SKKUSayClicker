using Firebase.Firestore;
using System;


[Serializable]
[FirestoreData]
public class FirebaseUpgradeSaveData
{
    [FirestoreProperty]
    public int[] Levels { get; private set; }
    [FirestoreProperty]
    public Timestamp LastSavedAt { get; private set; }

    public FirebaseUpgradeSaveData() { }

    public FirebaseUpgradeSaveData(UpgradeSaveData data)
    {
        Levels = data.Levels;
        LastSavedAt = data.LastSavedAt;
    }
}
