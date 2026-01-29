using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/Config/Currency Config")]
public class CurrencyConfigSO : ScriptableObject
{
    [System.Serializable]
    public class CurrencyEntry
    {
        public ECurrencyType Type;
        public float DefaultValue;
    }

    [SerializeField] private List<CurrencyEntry> _entries;

    public Dictionary<ECurrencyType, float> GetDefaults()
    {
        return _entries.ToDictionary(e => e.Type, e => e.DefaultValue);
    }
}
