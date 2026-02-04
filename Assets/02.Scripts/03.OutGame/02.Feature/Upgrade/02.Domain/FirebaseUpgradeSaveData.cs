using Firebase.Firestore;
using System;


[Serializable]
[FirestoreData]
public class FirebaseUpgradeSaveData
{
    [FirestoreProperty]
    public int[] Level { get; set; }

    public FirebaseUpgradeSaveData()
    {

    }
    public FirebaseUpgradeSaveData(UpgradeSaveData data)
    {
        Level = data.Level;
    }
}
