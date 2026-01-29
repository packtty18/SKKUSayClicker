using System;
using UnityEngine;

// 오직 데이터(재화)를 "관리"하는 클래스입니다.
// 클린 아키텍처에서는 "서비스"라는 이름을 쓴다. (그러나 게임에서는 보통 "매니저"라고 표현한다.)
public class CurrencyManager : LocalSingleton<CurrencyManager>
{
    // 이벤트
    public static SafeEvent<ECurrencyType> OnDataChanged;


    // 재화 데이터들 (배열로 관리)
    private SCurrency[] _currencies = new SCurrency[(int)ECurrencyType.Count];

    // 저장소
    // 의존이란 한 객체가 동작하기 위해서 다른 객체를 참조하는것을
    // DIP: 구현체에 의존하지 말고 약속에 의존해라 
    private ICurrencyRepository _repository;

    protected override void Init()
    {
        _repository = new LocalCurrencyRepository();


        SCurrency currency1 = new SCurrency(10000);
        SCurrency currency2 = new SCurrency(30000);
        SCurrency currency3 = currency1 + currency2;

        Debug.Log(currency3);  // 40k
    }

    private void Start()
    {
        float[] currencyValues = _repository.Load().Currencies;
        for (int i = 0; i < _currencies.Length; i++)
        {
            _currencies[i] = currencyValues[i];
        }

    }


    // 0. 재화 조회
    public SCurrency Get(ECurrencyType currencyType)
    {
        return _currencies[(int)currencyType];
    }

    // - 어쩔수 없는 재화 조회 편의 기능... ㅠㅠ

    public SCurrency Gold => Get(ECurrencyType.Money);
    public SCurrency Ruby => Get(ECurrencyType.Prestigy);

    // 1. 재화 추가
    public void Add(ECurrencyType type, SCurrency amount)
    {
        _currencies[(int)type] += amount;

        Save();

        OnDataChanged?.Invoke(type);
    }

    // 2. 재화 소모
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

    // 3. 돈 있으세요? 
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