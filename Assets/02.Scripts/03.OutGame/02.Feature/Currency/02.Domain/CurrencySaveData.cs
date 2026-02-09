using Firebase.Firestore;
using System;

[Serializable]
[FirestoreData]
public class CurrencySaveData
{
    [FirestoreProperty]
    public float[] Currencies { get; private set; }
    [FirestoreProperty]
    public long LastSavedAt { get; private set; }

    public CurrencySaveData() { }

    public CurrencySaveData(float[] currencies, long lastSavedAtUnix)
    {
        Currencies = currencies;
        LastSavedAt = lastSavedAtUnix;
    }


    public static CurrencySaveData CreateDefault()
    {
        return new CurrencySaveData(
            new float[(int)ECurrencyType.Count],
            0
        );
    }

    public static CurrencySaveData FromRuntime(Currency[] currencies)
    {
        float[] target = new float[currencies.Length];
        for (int i = 0; i < currencies.Length; i++)
            target[i] = (float)currencies[i];

        return new CurrencySaveData(
            target,
            DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        );
    }
}
