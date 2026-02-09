
using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class UpgradeManager : GlobalSingleton<UpgradeManager>
{
    public SafeEvent OnDataChanged = new();

    [SerializeField] private UpgradeSpecTableSO _specTable;

    //매니저 내부에서는 Upgrade 조작 가능
    private readonly Dictionary<EUpgradeType, Upgrade> _upgrades = new();
    public List<IReadOnlyUpgrade> Upgrades => _upgrades.Values.ToList<IReadOnlyUpgrade>();

    private IUpgradeRepository _repository;

    protected override void Init()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        _repository = new HybridUpgradeRepository();
#else
         _repository = new PlayerPrefsUpgradeRepository(AccountManager.Instance.Email);
#endif
        Load().Forget();
    }

    public void Save()
    {
        UpgradeSaveData saveData = UpgradeSaveData.FromRuntime(Upgrades.ToArray());
        _repository.Save(saveData).Forget();
    }

    private async UniTask Load()
    {
        try
        {
            UpgradeSaveData result;
            result = await _repository.Load();
            ApplyData(result);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[UpgradeManager] 로컬 로드 실패함: {e.Message}");
        }
    }

    private void ApplyData(UpgradeSaveData data)
    {
        int[] levels = data.Levels;
        foreach(var specData in _specTable.Datas)
        {
            if(_upgrades.ContainsKey(specData.Type))
            {
                throw new Exception($"Duplicate Upgrade Type: {specData.Type}");
            }
            _upgrades.Add(specData.Type, new Upgrade(specData, levels[(int)specData.Type]));
        }

        OnDataChanged?.Invoke();
        Debug.Log($"[CurrencyManager] Currency loaded. SavedAt: {data.LastSavedAt}");
    }

    public IReadOnlyUpgrade Get(EUpgradeType type)
    {
        _upgrades.TryGetValue(type, out var upgrade);
        return upgrade;
    }

    public bool CanLevelUp(EUpgradeType type)
    {
        if (!_upgrades.TryGetValue(type, out var upgrade))
            return false;

        if (!upgrade.CanLevelUp())
            return false;

        return CurrencyManager.Instance
            .CanAfford(upgrade.SpecData.CostType, upgrade.Cost);
    }

    public bool TryLevelUp(EUpgradeType type)
    {
        if (!CanLevelUp(type))
            return false;

        Upgrade upgrade = _upgrades[type];

        if (!CurrencyManager.Instance.TrySpend(upgrade.SpecData.CostType, upgrade.Cost))
            return false;

        if (!upgrade.TryLevelUp())
            return false;

        OnDataChanged?.Invoke();
        Save();   
        return true;
    }
}
