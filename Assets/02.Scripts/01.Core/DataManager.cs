using System.Collections.Generic;
using UnityEngine;
public enum ECurrentcyData
{
    Money,
    Prestigy,
}

public enum EIncomeData
{
    IncomeBonus,
    Theme1Price,
}

public enum EClickData
{
    ManualClickValue,
    AutoClickValue,

    DefaultPrintAutoDamageTime,
    DefaultAutoGetTime,

    CritRate,
    CritIncreaesRate,
}

public enum EPrinterData
{
    ValueByTime,
    DefaultProductTime,
}

public class DataManager : LocalSingleton<DataManager>
{
    private Dictionary<ECurrentcyData, DataValue<float>> _currency;
    private Dictionary<EIncomeData, DataValue<float>> _income;
    private Dictionary<EClickData, DataValue<float>> _click;
    private Dictionary<EPrinterData, DataValue<float>> _printer;

    protected override void Init()
    {
        Debug.Log("[DataManager] Init");
        _currency = new Dictionary<ECurrentcyData, DataValue<float>>()
        {
            { ECurrentcyData.Money,    new DataValue<float>(0f) },
            { ECurrentcyData.Prestigy, new DataValue<float>(0f) },
        };
        _income = new Dictionary<EIncomeData, DataValue<float>>()
        {
            { EIncomeData.IncomeBonus, new DataValue<float>(1f) },
            { EIncomeData.Theme1Price, new DataValue<float>(100f) },
        };
        _click = new Dictionary<EClickData, DataValue<float>>()
        {
            { EClickData.ManualClickValue,          new DataValue<float>(1f) },
            { EClickData.AutoClickValue,            new DataValue<float>(0.5f) },

            { EClickData.DefaultPrintAutoDamageTime,new DataValue<float>(5f) },
            { EClickData.DefaultAutoGetTime,        new DataValue<float>(5f) },

            { EClickData.CritRate,                  new DataValue<float>(0.1f) },
            { EClickData.CritIncreaesRate,          new DataValue<float>(1.2f) },
        };
        _printer = new Dictionary<EPrinterData, DataValue<float>>()
        {
            { EPrinterData.ValueByTime,        new DataValue<float>(1f) },
            { EPrinterData.DefaultProductTime, new DataValue<float>(60f) },
        };
    }

    public DataValue<float> GetData(ECurrentcyData data)
    {
        return _currency[data];
    }

    public DataValue<float> GetData(EIncomeData data)
    {
        return _income[data];
    }

    public DataValue<float> GetData(EClickData data)
    {
        return _click[data];
    }

    public DataValue<float> GetData(EPrinterData data)
    {
        return _printer[data];
    }

    public float GetDataValue(ECurrentcyData data)
    {
        return _currency[data].Value;
    }

    public float GetDataValue(EIncomeData data)
    {
        return _income[data].Value;
    }

    public float GetDataValue(EClickData data)
    {
        return _click[data].Value;
    }

    public float GetDataValue(EPrinterData data)
    {
        return _printer[data].Value;
    }
}
