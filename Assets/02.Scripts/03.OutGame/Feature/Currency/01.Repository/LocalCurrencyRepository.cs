using Sirenix.OdinInspector;
using UnityEngine;


public class LocalCurrencyRepository : ICurrencyRepository
{
    private const string DOMAIN = "Currency";
    private readonly string _userId;
    public LocalCurrencyRepository(string userId)
    {
        _userId = userId;
    }

    public void Save(SCurrencySaveData saveData)
    {
        // 어떻게든 Save한다.
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            PlayerPrefsRepository.SetFloat(_userId, DOMAIN, type.ToString(), saveData.Currencies[i]);//.ToString("G17")); //
        }
    }

    public SCurrencySaveData Load()
    {
        SCurrencySaveData data = SCurrencySaveData.Default;

        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;

            if (PlayerPrefs.HasKey($"{_userId}_{type.ToString()}"))
            {
                data.Currencies[i] =
                    PlayerPrefsRepository.GetFloat(_userId,DOMAIN,type.ToString(), 0);
            }
        }

        return data;
    }

    [Button("해당 User의 모든 데이터 삭제")]
    public void DeleteAll()
    {
        PlayerPrefsRepository.DeleteDomain(_userId, DOMAIN);
        Debug.Log($"{_userId}의 모든 {DOMAIN}데이터 제거");
    }
}