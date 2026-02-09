using Cysharp.Threading.Tasks;
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
        long convertedTime = saveData.LastSavedAt;
        UserScopedPlayerPrefs.SetString(
            _userId, DOMAIN, LAST_SAVED_AT_KEY, convertedTime.ToString()
        );


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

        long lastSavedAt = 0;
        if (PlayerPrefs.HasKey(PlayerPrefsKeyBuilder.GameData(_userId, DOMAIN, LAST_SAVED_AT_KEY)))
        {
            string savedLastTime = UserScopedPlayerPrefs.GetString(_userId,DOMAIN,LAST_SAVED_AT_KEY,"0");

            if (long.TryParse(savedLastTime, out long unix))
                lastSavedAt = unix;

        }

        CurrencySaveData data = new CurrencySaveData(currencies, lastSavedAt);
        return UniTask.FromResult(data);
    }

}