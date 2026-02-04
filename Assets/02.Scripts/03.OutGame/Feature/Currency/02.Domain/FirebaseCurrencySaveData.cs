using Firebase.Firestore;
using System;

[Serializable]
[FirestoreData]
public class FirebaseCurrencySaveData
{
    [FirestoreProperty]
    public float[] Currencies { get; private set; }

    public FirebaseCurrencySaveData()
    {

    }
    public FirebaseCurrencySaveData(SCurrencySaveData data)
    {
        Currencies = data.Currencies;
    }
}
