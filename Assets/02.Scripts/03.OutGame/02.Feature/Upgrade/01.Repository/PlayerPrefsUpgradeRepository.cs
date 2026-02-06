using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using MoreMountains.Tools;
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
        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;
            UserScopedPlayerPrefs.SetInt(
                _userId,
                DOMAIN,
                type.ToString(),
                saveData.Levels[i]
            );
        }

        //TimeStamp => DateTime => UTC 정규화 => long 형식으로 바꾸기 => tostring
        long convertedTime = saveData.LastSavedAt.ToDateTime().ToUniversalTime().Ticks;

        UserScopedPlayerPrefs.SetString(_userId, DOMAIN, LAST_SAVED_AT_KEY, convertedTime.ToString());

        return UniTask.CompletedTask;
    }

    public UniTask<UpgradeSaveData> Load()
    {
        UpgradeSaveData data = UpgradeSaveData.Default;

        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;

            if (PlayerPrefs.HasKey(PlayerPrefsKeyBuilder.GameData(_userId, DOMAIN, type.ToString())))
            {
                data.Levels[i] =
                    UserScopedPlayerPrefs.GetInt(_userId, DOMAIN, type.ToString());
            }
        }

        if (PlayerPrefs.HasKey(PlayerPrefsKeyBuilder.GameData(_userId, DOMAIN, LAST_SAVED_AT_KEY)))
        {
            string savedLastTime = UserScopedPlayerPrefs.GetString(_userId, DOMAIN, LAST_SAVED_AT_KEY, "0");

            //시간을 string => long => dataTime => timestamp
            if (long.TryParse(savedLastTime, out long ticks))
            {
                DateTime utcTime = new DateTime(ticks, DateTimeKind.Utc);
                data.LastSavedAt = Timestamp.FromDateTime(utcTime);
            }
        }

        return UniTask.FromResult(data);
    }

}
