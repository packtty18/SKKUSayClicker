
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : GlobalSingleton<UpgradeManager>, ISaveManager
{
    public SafeEvent OnDataChanged = new();

    [SerializeField] private UpgradeSpecTableSO _specTable;

    //매니저 내부에서는 Upgrade 조작 가능
    private readonly Dictionary<EUpgradeType, Upgrade> _upgrades = new();
    //private List<Upgrade> Upgrades => _upgrades.Values.ToList<Upgrade>();

    //외부에서는 함부로 Upgrade를 수정하지 못하도록 IReadonly로 주기
    public List<IReadOnlyUpgrade> Upgrades => _upgrades.Values.ToList<IReadOnlyUpgrade>();

    private IUpgradeRepository _repository;
    protected override void Init()
    {
        _repository = new FirebaseUpgradeRepository();
        Load().Forget();
    }

    protected async UniTask Load()
    {
        var result = await _repository.Load();
        int[] levels = result.Level;

        foreach (var specData in _specTable.Datas)
        {
            if (_upgrades.ContainsKey(specData.Type))
                throw new Exception($"Duplicate Upgrade Type: {specData.Type}");

            _upgrades.Add(specData.Type, new Upgrade(specData, levels[(int)specData.Type]));
        }

        OnDataChanged?.Invoke();
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

        CurrencyManager.Instance
            .TrySpend(upgrade.SpecData.CostType, upgrade.Cost);

        upgrade.TryLevelUp();

        OnDataChanged?.Invoke();
        Save();
        return true;
    }

    public void Save()
    {
        var saveData = new UpgradeSaveData();
        saveData.Level = new int[_upgrades.Count];
        foreach (var specData in _upgrades)
        {
            EUpgradeType type = specData.Key;
            int level = specData.Value.Level;

            saveData.Level[(int)type] = level;
        }
        _repository.Save(saveData);
    }
}
