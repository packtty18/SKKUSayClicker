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
    public async UniTaskVoid Save(SUpgradeSaveData upgrade)
    {
        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;
            PlayerPrefsRepository.SetInt(
                _userId,
                DOMAIN,
                type.ToString(),
                upgrade.Level[i]
            );
        }
    }

    public async UniTask<SUpgradeSaveData> Load()
    {
        SUpgradeSaveData data = SUpgradeSaveData.Default;

        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;

            if (PlayerPrefs.HasKey($"{_userId}_{DOMAIN}_{type.ToString()}"))
            {
                data.Level[i] =
                    PlayerPrefsRepository.GetInt(_userId,DOMAIN, type.ToString(), 0);
            }
        }

        return data;
    }
}
