using Firebase.Firestore;
using System;

public struct CurrencySaveData
{
    public float[] Currencies;

    // 재화 기본값
    public static CurrencySaveData Default => new CurrencySaveData()
    {
        Currencies = new float[(int)ECurrencyType.Count]
    };
}