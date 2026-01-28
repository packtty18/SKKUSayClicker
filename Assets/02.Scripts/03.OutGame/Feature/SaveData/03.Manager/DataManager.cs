using System.Collections.Generic;
using UnityEngine;

public enum EIncomeData
{
    IncomeBonus,
    Theme1Price,


    Count,
}

public enum EClickData
{
    ManualClickValue,
    AutoClickValue,

    DefaultPrintAutoDamageTime,
    DefaultAutoGetTime,

    CritRate,
    CritIncreaesRate,


    Count,
}

public enum EPrinterData
{
    ValueByTime,
    DefaultProductTime,


    Count,
}

public class DataManager : GlobalSingleton<DataManager>
{
    //변경 가능(세이브 시스템)
    private Dictionary<ECurrentcyData, DataValue<float>> _currency; //현재 재화의 데이터
    private ICurrencyRepository _repository;
    public ICurrencyRepository Repository => _repository;


    //고정 데이터(SO)
    private Dictionary<EIncomeData, DataValue<float>> _income;      //획득에 대한 데이터(가격 및 획득증가량)
    private Dictionary<EClickData, DataValue<float>> _click;        //클릭요소에 대한 정보(분리예정)
    private Dictionary<EPrinterData, DataValue<float>> _printer;    //프린터에 관한 데이터

    protected override void Init()
    {
        Debug.Log("[DataManager] Init");

#if UNITY_EDITOR
        _repository = new LocalCurrencyRepository();
#else
//빌드 환경일 경우 로컬이 아닌 파이어베이스를 통한 리파지토리
    _repository = new LocalCurrencyRepository();
#endif
        SCurrencySaveData save = _repository.Load();
        _currency = new Dictionary<ECurrentcyData, DataValue<float>>()
        {
            { ECurrentcyData.Money,    new DataValue<float>(save.Currencies[0])},
            { ECurrentcyData.Prestigy, new DataValue<float>(save.Currencies[1])},
        };


        _income = new Dictionary<EIncomeData, DataValue<float>>()
        {
            { EIncomeData.IncomeBonus, new DataValue<float>(1f) },
            { EIncomeData.Theme1Price, new DataValue<float>(100f) },
        };
        _click = new Dictionary<EClickData, DataValue<float>>()
        {
            { EClickData.ManualClickValue,          new DataValue<float>(10f) },
            { EClickData.AutoClickValue,            new DataValue<float>(2f) },

            { EClickData.DefaultPrintAutoDamageTime,new DataValue<float>(5f) },
            { EClickData.DefaultAutoGetTime,        new DataValue<float>(5f) },

            { EClickData.CritRate,                  new DataValue<float>(0.1f) },
            { EClickData.CritIncreaesRate,          new DataValue<float>(1.2f) },
        };
        _printer = new Dictionary<EPrinterData, DataValue<float>>()
        {
            { EPrinterData.ValueByTime,        new DataValue<float>(1f) },
            { EPrinterData.DefaultProductTime, new DataValue<float>(100) },
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
