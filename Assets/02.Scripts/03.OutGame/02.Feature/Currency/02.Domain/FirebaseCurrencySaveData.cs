using Firebase.Firestore;
using System;

[Serializable]
[FirestoreData]
public class FirebaseCurrencySaveData
{
    [FirestoreProperty]
    public float[] Currencies { get; private set; }
    [FirestoreProperty]
    public Timestamp LastSavedAt { get; private set; }

    public FirebaseCurrencySaveData() { }

    public FirebaseCurrencySaveData(CurrencySaveData data)
    {
        Currencies = data.Currencies;
        LastSavedAt = data.LastSavedAt;
    }
}
