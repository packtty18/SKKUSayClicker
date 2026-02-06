using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using System;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerPrefsCurrencyRepository : ICurrencyRepository
{
    private const string DOMAIN = "Currency";
    private const string LAST_SAVED_AT_KEY = "LastSavedAt";

    private readonly string _userId;
    public PlayerPrefsCurrencyRepository(string userId)
    {
        _userId = userId;
    }

    public UniTask Save(CurrencySaveData saveData)
    {
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            UserScopedPlayerPrefs.SetFloat(
                _userId,
                DOMAIN,
                type.ToString(),
                saveData.Currencies[i]
            );
        }

        //TimeStamp => DateTime => UTC 정규화 => long 형식으로 바꾸기 => tostring
        long convertedTime = saveData.LastSavedAt.ToDateTime().ToUniversalTime().Ticks;


        UserScopedPlayerPrefs.SetString( _userId,DOMAIN,LAST_SAVED_AT_KEY,convertedTime.ToString());

        return UniTask.CompletedTask;
    }


    public UniTask<CurrencySaveData> Load()
    {
        float[] currencies = new float[(int)ECurrencyType.Count];
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;

            if (PlayerPrefs.HasKey(PlayerPrefsKeyBuilder.GameData(_userId, DOMAIN, type.ToString())))
            {
                currencies[i] = UserScopedPlayerPrefs.GetFloat(_userId, DOMAIN, type.ToString());
            }
        }

        Timestamp timestamp = Timestamp.FromDateTime(DateTime.MinValue);
        if (PlayerPrefs.HasKey(PlayerPrefsKeyBuilder.GameData(_userId, DOMAIN, LAST_SAVED_AT_KEY)))
        {
            string savedLastTime = UserScopedPlayerPrefs.GetString(_userId,DOMAIN,LAST_SAVED_AT_KEY,"0");

            //시간을 string => long => dataTime => timestamp
            if (long.TryParse(savedLastTime, out long ticks))
            {
                DateTime utcTime = new DateTime(ticks, DateTimeKind.Utc);
                timestamp = Timestamp.FromDateTime(utcTime);
            }
            
        }

        CurrencySaveData data = new CurrencySaveData(currencies, timestamp);
        return UniTask.FromResult(data);
    }

}