using Firebase.Firestore;
using System;

public struct CurrencySaveData
{
    public float[] Currencies;
    public Timestamp LastSavedAt;
    public static CurrencySaveData Default => new CurrencySaveData()
    {
        Currencies = new float[(int)ECurrencyType.Count],
        LastSavedAt = Timestamp.FromDateTime(DateTime.MinValue),
    };

    public static CurrencySaveData FromFirebase(FirebaseCurrencySaveData firebaseData)
    {
        if (firebaseData == null)
        {
            return Default;
        }

        return new CurrencySaveData
        {
            Currencies = firebaseData.Currencies,
            LastSavedAt = firebaseData.LastSavedAt,
        };
    }
}