using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyRepository : StructDataRepository<ECurrencyType, SCurrency>
{
    public void InitializeFromConfig(CurrencyConfigSO config)
    {
        var defaults = config.GetDefaults();
        foreach (var kvp in defaults)
        {
            Set(kvp.Key, new SCurrency(kvp.Value));
        }
    }
    public Dictionary<string, float> GetSaveData()
    {
        var result = new Dictionary<string, float>();
        foreach (var key in GetAllKeys())
        {
            result[key.ToString()] = Get(key).Value;
        }
        return result;
    }

    public void LoadSaveData(Dictionary<string, float> data)
    {
        foreach (var kvp in data)
        {
            if (Enum.TryParse<ECurrencyType>(kvp.Key, out var key))
            {
                //고쳐야함
                //Get(key) = new Currency(kvp.Value);
            }
        }
    }

    public float GetMoneyValue() => Get(ECurrencyType.Money).Value;
}