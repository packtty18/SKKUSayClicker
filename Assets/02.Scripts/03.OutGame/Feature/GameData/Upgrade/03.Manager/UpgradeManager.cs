
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : GloblaManager<UpgradeManager>, ISaveManager
{
    public SafeEvent OnDataChanged = new();

    [SerializeField] private UpgradeSpecTableSO _specTable;

    private readonly Dictionary<EUpgradeType, Upgrade> _upgrades = new();
    public IReadOnlyDictionary<EUpgradeType, Upgrade> Upgrades => _upgrades;


    private IUpgradeRepository _repository;
    protected override void Init()
    {
        _repository = new LocalUpgradeRepository();
        int[] levels = _repository.Load().Level;

        foreach (var specData in _specTable.Datas)
        {
            if (_upgrades.ContainsKey(specData.Type))
                throw new Exception($"Duplicate Upgrade Type: {specData.Type}");

            _upgrades.Add(specData.Type, new Upgrade(specData, levels[(int)specData.Type]));
        }

        OnDataChanged?.Invoke();
    }
    public Upgrade Get(EUpgradeType type)
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
        var saveData = new SUpgradeSaveData();
        saveData.Level = new int[_upgrades.Count];
        foreach (var specData in _upgrades)
        {
            EUpgradeType type = specData.Key;
            int level = specData.Value.Level;

            saveData.Level[(int)type] = level;
        }
        _repository.Save(saveData);
    }

    [Button("세이브 삭제")]
    public void ResetSave()
    {
        _repository.DeleteAllSave();
    }
}
