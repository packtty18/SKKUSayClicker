using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class CurrencyManager : GlobalSingleton<CurrencyManager>
{
    public static SafeEvent<ECurrencyType> OnDataChanged = new();

    private Currency[] _currencies = new Currency[(int)ECurrencyType.Count];
    private ICurrencyRepository _repository;

    public Currency Money => Get(ECurrencyType.Money);
    public Currency Prestigy => Get(ECurrencyType.Prestigy);

    protected override void Init()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        _repository = new HybridCurrencyRepository();
#else
         _repository = new PlayerPrefsCurrencyRepository(AccountManager.Instance.Email);
#endif

        Load().Forget();
    }
    public void Save()
    {
        CurrencySaveData saveData = CurrencySaveData.FromRuntime(_currencies);
        //하이브리드 리포지토리로 저장 위임
        _repository.Save(saveData).Forget();
    }

    private async UniTask Load()
    {
        try
        {
            CurrencySaveData result;
            result = await _repository.Load();
            ApplyData(result);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[CurrencyManager] 재화 로드 실패함: {e.Message}");
        }
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

   
}