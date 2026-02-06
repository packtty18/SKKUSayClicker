using Firebase.Firestore;
using System;

[Serializable]
[FirestoreData]
public class CurrencySaveData
{
    [FirestoreProperty]
    public float[] Currencies { get; private set; }
    [FirestoreProperty]
    public Timestamp LastSavedAt { get; private set; }

    public CurrencySaveData() { }

    //로드용도
    public CurrencySaveData(float[] currencies, Timestamp lastSavedAt)
    {
        Currencies = currencies;
        LastSavedAt = lastSavedAt;
    }


    //세이브 전용. 초기 상태 생성하기
    public static CurrencySaveData CreateDefault()
    {
        return new CurrencySaveData(new float[(int)ECurrencyType.Count], 
            Timestamp.FromDateTime(DateTime.UtcNow));
    }

    //현재 런타임 데이터를 세이브 데이터로 바꾸기
    public static CurrencySaveData FromRuntime(Currency[] currencies)
    {
        float[] target = new float[currencies.Length];
        for (int i = 0; i < currencies.Length; i++)
        {
            target[i] = (float)currencies[i];
        }
        
        return new CurrencySaveData(
            target,
            Timestamp.FromDateTime(DateTime.UtcNow)
        );
    }
}
