using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerPrefsCurrencyRepository : ICurrencyRepository
{
    private const string DOMAIN = "Currency";
    private readonly string _userId;
    public PlayerPrefsCurrencyRepository(string userId)
    {
        _userId = userId;
    }

    public async UniTaskVoid Save(SCurrencySaveData saveData)
    {
        // 어떻게든 Save한다.
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            UserScopedPlayerPrefs.SetFloat(_userId, DOMAIN, type.ToString(), saveData.Currencies[i]);//.ToString("G17")); //
        }
    }

    public async UniTask<SCurrencySaveData> Load()
    {
        SCurrencySaveData data = SCurrencySaveData.Default;

        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;

            if (PlayerPrefs.HasKey($"{_userId}_{type.ToString()}"))
            {
                data.Currencies[i] =
                    UserScopedPlayerPrefs.GetFloat(_userId,DOMAIN,type.ToString(), 0);
            }
        }

        return data;
    }
}