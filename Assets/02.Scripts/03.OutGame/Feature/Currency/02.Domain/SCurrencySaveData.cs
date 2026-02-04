using Firebase.Firestore;
using System;

public struct SCurrencySaveData
{
    public float[] Currencies;

    // 재화 기본값
    public static SCurrencySaveData Default => new SCurrencySaveData()
    {
        Currencies = new float[(int)ECurrencyType.Count]
    };
}


[Serializable]
[FirestoreData]
public class FirebaseCurrencySaveData
{
    [FirestoreProperty]
    public float[] Currencies { get; set; }
}