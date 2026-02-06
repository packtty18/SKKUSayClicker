using Cysharp.Threading.Tasks;
using MoreMountains.Tools;
using UnityEngine;

public class PlayerPrefsUpgradeRepository : IUpgradeRepository
{
    private const string DOMAIN = "Upgrade"; 
    private readonly string _userId;
    public PlayerPrefsUpgradeRepository(string userId)
    {
        _userId = userId;
    }
    public UniTask Save(UpgradeSaveData upgrade)
    {
        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;
            UserScopedPlayerPrefs.SetInt(
                _userId,
                DOMAIN,
                type.ToString(),
                upgrade.Level[i]
            );
        }

        return UniTask.CompletedTask;
    }

    public UniTask<UpgradeSaveData> Load()
    {
        UpgradeSaveData data = UpgradeSaveData.Default;

        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;

            if (PlayerPrefs.HasKey($"{_userId}_{DOMAIN}_{type.ToString()}"))
            {
                data.Level[i] =
                    UserScopedPlayerPrefs.GetInt(_userId,DOMAIN, type.ToString(), 0);
            }
        }

        return UniTask.FromResult(data);
    }
}
