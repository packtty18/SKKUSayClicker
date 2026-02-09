using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using System;
using UnityEngine;

public class PlayerPrefsUpgradeRepository : IUpgradeRepository
{
    private const string DOMAIN = "Upgrade";
    private const string LAST_SAVED_AT_KEY = "LastSavedAt";
    private readonly string _userId;
    public PlayerPrefsUpgradeRepository(string userId)
    {
        _userId = userId;
    }

    public UniTask Save(UpgradeSaveData saveData)
    {
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            UserScopedPlayerPrefs.SetFloat(
                _userId,
                DOMAIN,
                type.ToString(),
                saveData.Levels[i]
            );
        }

        UserScopedPlayerPrefs.SetString(_userId, DOMAIN, LAST_SAVED_AT_KEY, saveData.LastSavedAt.ToString());

        return UniTask.CompletedTask;
    }


    public UniTask<UpgradeSaveData> Load()
    {
        int[] levels = new int[(int)EUpgradeType.Count];
        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;

            if (PlayerPrefs.HasKey(PlayerPrefsKeyBuilder.GameData(_userId, DOMAIN, type.ToString())))
            {
                levels[i] = UserScopedPlayerPrefs.GetInt(_userId, DOMAIN, type.ToString());
            }
        }

        long lastSavedAt = 0;
        if (PlayerPrefs.HasKey(PlayerPrefsKeyBuilder.GameData(_userId, DOMAIN, LAST_SAVED_AT_KEY)))
        {
            string savedLastTime = UserScopedPlayerPrefs.GetString(_userId, DOMAIN, LAST_SAVED_AT_KEY, "0");

            if (long.TryParse(savedLastTime, out long unix))
                lastSavedAt = unix;

        }
        UpgradeSaveData data = new UpgradeSaveData(levels, lastSavedAt);
        return UniTask.FromResult(data);
    }
}
