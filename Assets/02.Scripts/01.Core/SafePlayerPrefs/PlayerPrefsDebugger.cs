using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerPrefsDebugger : GlobalSingleton<PlayerPrefsDebugger>
{
    [ShowInInspector, ReadOnly]
    private List<AccountDebugData> _accountDataList = new List<AccountDebugData>();

    protected override void Init()
    {
        RefreshData();
    }

    [Button("Refresh", ButtonSizes.Large)]
    private void RefreshData()
    {
        _accountDataList.Clear();

        // 모든 유저 가져오기
        var allUserIds = PlayerPrefsIndex.GetAllUserIds();

        foreach (var userId in allUserIds)
        {
            var accountData = new AccountDebugData
            {
                UserId = userId,
                CurrencyData = new Dictionary<string, string>(),
                UpgradeData = new Dictionary<string, string>(),
                AccountData = new Dictionary<string, string>(),
                OtherData = new Dictionary<string, string>()
            };

            // 유저의 모든 키 가져오기
            var keys = PlayerPrefsIndex.GetKeys(userId);

            foreach (var key in keys)
            {
                if (!PlayerPrefs.HasKey(key))
                    continue;

                // 키 파싱: {userId}_{domain}_{dataType}
                string[] parts = key.Split('_');
                
                if (parts.Length >= 3)
                {
                    string domain = parts[1];
                    string dataType = string.Join("_", parts, 2, parts.Length - 2);

                    // 값 가져오기 (타입별로 시도)
                    string value = GetValueAsString(key);

                    // Domain별로 분류
                    switch (domain)
                    {
                        case "Currency":
                            accountData.CurrencyData[dataType] = value;
                            break;
                        case "Upgrade":
                            accountData.UpgradeData[dataType] = value;
                            break;
                        case "Account":
                            accountData.AccountData[dataType] = value;
                            break;
                        default:
                            accountData.OtherData[$"{domain}_{dataType}"] = value;
                            break;
                    }
                }
            }

            _accountDataList.Add(accountData);
        }

        Debug.Log($"[PlayerPrefsDebugger] Data refreshed - {_accountDataList.Count} accounts loaded");
    }

    private string GetValueAsString(string key)
    {
        // Int 시도
        if (PlayerPrefs.HasKey(key))
        {
            try
            {
                int intValue = PlayerPrefs.GetInt(key, int.MinValue);
                if (intValue != int.MinValue)
                    return intValue.ToString();
            }
            catch { }

            // Float 시도
            try
            {
                float floatValue = PlayerPrefs.GetFloat(key, float.MinValue);
                if (!float.IsNaN(floatValue) && floatValue != float.MinValue)
                    return floatValue.ToString("F2");
            }
            catch { }

            // String 시도
            return PlayerPrefs.GetString(key, "<empty>");
        }

        return "<not found>";
    }

    [Button("Clear All PlayerPrefs (Danger)", ButtonSizes.Large)]
    [GUIColor(1, 0.3f, 0.3f)]
    private void ClearAll()
    {
        PlayerPrefsRepository.ResetAll();
        RefreshData();
        Debug.LogWarning("[PlayerPrefsDebugger] All PlayerPrefs cleared!");
    }

    [Button("Delete Specific Account", ButtonSizes.Medium)]
    private void DeleteAccount(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("[PlayerPrefsDebugger] UserId is empty!");
            return;
        }

        PlayerPrefsRepository.DeleteUser(userId);
        RefreshData();
        Debug.Log($"[PlayerPrefsDebugger] Account deleted: {userId}");
    }

    [Serializable]
    public class AccountDebugData
    {
        [ShowInInspector, ReadOnly, TitleGroup("Account Info")]
        public string UserId;

        [ShowInInspector, ReadOnly, FoldoutGroup("Currency Data")]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
        public Dictionary<string, string> CurrencyData;

        [ShowInInspector, ReadOnly, FoldoutGroup("Upgrade Data")]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
        public Dictionary<string, string> UpgradeData;

        [ShowInInspector, ReadOnly, FoldoutGroup("Account Data")]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
        public Dictionary<string, string> AccountData;

        [ShowInInspector, ReadOnly, FoldoutGroup("Other Data")]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
        public Dictionary<string, string> OtherData;
    }
}
