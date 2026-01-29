using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class UpgradeManager : LocalSingleton<UpgradeManager>
{
    public static event Action OnDataChanged;

    [SerializeField] private UpgradeSpecTableSO _specTable;

    private Dictionary<EUpgradeType, Upgrade> _upgrades = new();
    public Dictionary<EUpgradeType, Upgrade> GetUpgrade => _upgrades;
    protected override void Init()
    {
        // 스펙 데이터에 따라 도메인 생성
        foreach (var specData in _specTable.Datas)
        {
            if (_upgrades.ContainsKey(specData.Type))
            {
                throw new Exception($"There is already an upgrade with type {specData.Type}");
            }

            _upgrades.Add(specData.Type, new Upgrade(specData));
        }

        OnDataChanged?.Invoke();
    }

    public bool CanLevelUp(EUpgradeType type)
    {
        if (!_upgrades.TryGetValue(type, out Upgrade upgrade))
        {
            return false;
        }

        if(upgrade.IsMaxLevel)
        {
            return false;
        }

        return true;
    }

    public bool TryLevelUp(EUpgradeType type)
    {
        if (!_upgrades.TryGetValue(type, out Upgrade upgrade))
        {
            return false;
        }

        if (upgrade.TryLevelUp())
        {
            return true;
        }

        return false;
    }


}
