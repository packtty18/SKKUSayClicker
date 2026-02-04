using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class CurrencyManager : GlobalSingleton<CurrencyManager>, ISaveManager
{
    public static SafeEvent<ECurrencyType> OnDataChanged = new();

    private SCurrency[] _currencies = new SCurrency[(int)ECurrencyType.Count];
    private ICurrencyRepository _repository;

    protected override void Init()
    {
        _repository = new FirebaseCurrencyRepository();
        Load().Forget();
    }

    private async UniTask Load()
    {
        var result  = await _repository.Load();
        float[] currencyValues = result.Currencies;
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

        _repository.Save(saveData).Forget();
    }
}