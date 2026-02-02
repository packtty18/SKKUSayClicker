using MoreMountains.Tools;
using UnityEngine;

public class LocalUpgradeRepository : IUpgradeRepository
{
    private const string DOMAIN = "Upgrade"; 
    private readonly string _userId;
    public LocalUpgradeRepository(string userId)
    {
        _userId = userId;
    }
    public void Save(SUpgradeSaveData upgrade)
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

    public SUpgradeSaveData Load()
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

    public void DeleteAll()
    {
        PlayerPrefsRepository.DeleteDomain(_userId, DOMAIN);
        Debug.Log($"{_userId}의 모든 {DOMAIN}데이터 제거");
    }

}
