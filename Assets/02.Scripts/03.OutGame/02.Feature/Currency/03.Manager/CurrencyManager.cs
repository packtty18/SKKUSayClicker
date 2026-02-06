using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CurrencyManager : GlobalSingleton<CurrencyManager>
{
    public static SafeEvent<ECurrencyType> OnDataChanged = new();

    private Currency[] _currencies = new Currency[(int)ECurrencyType.Count];
    private ICurrencyRepository _hybridRepository;

    public Currency Money => Get(ECurrencyType.Money);
    public Currency Prestigy => Get(ECurrencyType.Prestigy);

    protected override void Init()
    {
        _hybridRepository = new HybridCurrencyRepository();
        Load().Forget();
    }

    private async UniTask Load()
    {
        CurrencySaveData result = CurrencySaveData.Default;

        try
        {
            result = await _hybridRepository.Load();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[CurrencyManager] 서버 로드 실패함: {e.Message}");
        }

        ApplyData(result);
    }

    private void ApplyData(CurrencySaveData data)
    {
        for (int i = 0; i < _currencies.Length; i++)
        {
            float value = 0f;

            if (data.Currencies != null && i < data.Currencies.Length)
            {
                value = data.Currencies[i];
            }

            _currencies[i] = value;
            OnDataChanged.Invoke((ECurrencyType)i);
        }

        Debug.Log($"[CurrencyManager] Currency loaded. SavedAt: {data.LastSavedAt}");
    }

    public Currency Get(ECurrencyType currencyType)
    {
        return _currencies[(int)currencyType];
    }

    public void Add(ECurrencyType type, Currency amount)
    {
        _currencies[(int)type] += amount;

        Save();

        OnDataChanged?.Invoke(type);
    }

    public bool TrySpend(ECurrencyType type, Currency amount)
    {
        if (_currencies[(int)type] >= amount)
        {
            _currencies[(int)type] -= amount;

            Save();

            OnDataChanged?.Invoke(type);

            return true;
        }

        return false;
    }

    public bool CanAfford(ECurrencyType type, Currency amount)
    {
        return _currencies[(int)type] >= amount;
    }

    public UniTask Save()
    {
        //세이브 데이터 제작
        var saveData = CurrencySaveData.Default;
        for (int i = 0; i < _currencies.Length; i++)
        {
            saveData.Currencies[i] = (float)_currencies[i];
        }
        saveData.LastSavedAt = Timestamp.FromDateTime(DateTime.UtcNow);

        //하이브리드 리포지토리로 저장 위임
        _hybridRepository.Save(saveData).Forget();
        return UniTask.CompletedTask;
    }
}