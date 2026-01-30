using Sirenix.OdinInspector;

public class CurrencyManager : GloblaManager<CurrencyManager>, ISaveManager
{

    public static SafeEvent<ECurrencyType> OnDataChanged = new();

    private SCurrency[] _currencies = new SCurrency[(int)ECurrencyType.Count];
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

    public SCurrency Money => Get(ECurrencyType.Money);
    public SCurrency Prestigy => Get(ECurrencyType.Prestigy);

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

    public void Save()
    {
        var saveData = new SCurrencySaveData();
        saveData.Currencies = new float[_currencies.Length];
        for (int i = 0; i < _currencies.Length; i++)
        {
            saveData.Currencies[i] = (float)_currencies[i];
        }
        _repository.Save(saveData);
    }

    [Button("세이브 삭제")]
    public void ResetSave()
    {
        _repository.DeleteAllSave();
    }
}