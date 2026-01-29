using System;
using UnityEngine;

// 오직 데이터(재화)를 "관리"하는 클래스입니다.
// 클린 아키텍처에서는 "서비스"라는 이름을 쓴다. (그러나 게임에서는 보통 "매니저"라고 표현한다.)
public class CurrencyManager : LocalSingleton<CurrencyManager>
{
    // 이벤트
    public static SafeEvent<ECurrencyType> OnDataChanged = new();


    // 재화 데이터들 (배열로 관리)
    private SCurrency[] _currencies = new SCurrency[(int)ECurrencyType.Count];

    // 저장소
    // 의존이란 한 객체가 동작하기 위해서 다른 객체를 참조하는것을
    // DIP: 구현체에 의존하지 말고 약속에 의존해라 
    private ICurrencyRepository _repository;

    protected override void Init()
    {
        _repository = new LocalCurrencyRepository();

    }

    private void Start()
    {
        float[] currencyValues = _repository.Load().Currencies;
        for (int i = 0; i < _currencies.Length; i++)
        {
            _currencies[i] = currencyValues[i];
        }

    }

    public SCurrency Get(ECurrencyType currencyType)
    {
        return _currencies[(int)currencyType];
    }

    public SCurrency Gold => Get(ECurrencyType.Money);
    public SCurrency Ruby => Get(ECurrencyType.Prestigy);

    public void Add(ECurrencyType type, SCurrency amount)
    {
        _currencies[(int)type] += amount;

        Save();

        OnDataChanged?.Invoke(type);
    }

    public bool TrySpend(ECurrencyType type, SCurrency amount)
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

    public bool CanAfford(ECurrencyType type, SCurrency amount)
    {
        return _currencies[(int)type] >= amount;
    }

    private void Save()
    {
        var saveData = new SCurrencySaveData();
        saveData.Currencies = new float[_currencies.Length];
        for (int i = 0; i < _currencies.Length; i++)
        {
            saveData.Currencies[i] = (float)_currencies[i];
        }
        _repository.Save(saveData);
    }


}