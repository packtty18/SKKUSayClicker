using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using System;
using System.Threading;
using UnityEngine;

public class CurrencyManager : GlobalSingleton<CurrencyManager>
{
    public static SafeEvent<ECurrencyType> OnDataChanged = new();

    private Currency[] _currencies = new Currency[(int)ECurrencyType.Count];
    private ICurrencyRepository _serverRepository;
    private ICurrencyRepository _localRepository;

    protected override void Init()
    {
        _serverRepository = new FirebaseCurrencyRepository();
        _localRepository = new PlayerPrefsCurrencyRepository(AccountManager.Instance.Email);
        Load().Forget();
    }

    private async UniTask Load()
    {
        CurrencySaveData serverResult = CurrencySaveData.Default;
        CurrencySaveData localResult = CurrencySaveData.Default;

        try
        {
            serverResult = await _serverRepository.Load();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[CurrencyManager] 서버 로드 실패함: {e.Message}");
        }

        try
        {
            localResult = await _localRepository.Load();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[CurrencyManager] 로컬 로드 실패함: {e.Message}");
        }

        CurrencySaveData result = SelectLatest(serverResult, localResult);
        ApplyCurrencies(result);
    }

    private CurrencySaveData SelectLatest(
    CurrencySaveData server,
    CurrencySaveData local)
    {
        bool isServerLatest = server.LastSavedAt >= local.LastSavedAt;

        Debug.Log(
            $"[CurrencyManager] {(isServerLatest ? "서버" : "로컬")} 선택. " +
            $"ServerTime: {server.LastSavedAt.ToDateTime():O}, " +
            $"LocalTime: {local.LastSavedAt.ToDateTime():O}"
        );

        return isServerLatest ? server : local;
    }


    private void ApplyCurrencies(CurrencySaveData data)
    {
        for (int i = 0; i < _currencies.Length; i++)
        {
            float value = 0f;

            if (data.Currencies != null && i < data.Currencies.Length)
            {
                value = data.Currencies[i];
            }

            _currencies[i] = value;
            OnDataChanged.Invoke((ECurrencyType)i);
        }

        Debug.Log($"[CurrencyManager] Currency loaded. SavedAt: {data.LastSavedAt}");
    }

    public Currency Get(ECurrencyType currencyType)
    {
        return _currencies[(int)currencyType];
    }

    public Currency Money => Get(ECurrencyType.Money);
    public Currency Prestigy => Get(ECurrencyType.Prestigy);

    public void Add(ECurrencyType type, Currency amount)
    {
        _currencies[(int)type] += amount;

        RequestSave();

        OnDataChanged?.Invoke(type);
    }

    public bool TrySpend(ECurrencyType type, Currency amount)
    {
        if (_currencies[(int)type] >= amount)
        {
            _currencies[(int)type] -= amount;

            RequestSave();

            OnDataChanged?.Invoke(type);

            return true;
        }

        return false;
    }

    public bool CanAfford(ECurrencyType type, Currency amount)
    {
        return _currencies[(int)type] >= amount;
    }

    [SerializeField] private float _saveDelaySeconds = 0.6f;
    [SerializeField] private int _firebaseSaveInterval = 5;

    private CancellationTokenSource _saveCts;
    private int _saveCount;

    private void RequestSave()
    {
        _saveCts?.Cancel();     //이전 예약이 있다면 취소
        _saveCts?.Dispose();    //완전히 리소스를 해제. 이후 

        _saveCts = new CancellationTokenSource();

        //해당 토큰을 사용하는 비동기 함수 실행
        SaveWithDelay(_saveCts.Token).Forget();
    }

    private async UniTaskVoid SaveWithDelay(CancellationToken token)
    {
        //도중에 토큰이 Cancel 되면 catch 실행.
        try
        {
            //세이브 대기시간 동안 대기 + Cancellation 토큰을 등록함
            await UniTask.Delay(
                TimeSpan.FromSeconds(_saveDelaySeconds)
                ,cancellationToken: token);

            ExecuteSave();
        }
        catch (OperationCanceledException)
        {
            // 정상적인 취소 (새 Save 요청 들어옴)
            // 아직 취소로직은 없음 그냥 세이브 안함
        }
    }

    private void ExecuteSave()
    {
        _saveCount++;
        var saveData = CurrencySaveData.Default;

        for (int i = 0; i < _currencies.Length; i++)
            saveData.Currencies[i] = (float)_currencies[i];

        saveData.LastSavedAt = Timestamp.FromDateTime(DateTime.UtcNow);


        if (_saveCount % _firebaseSaveInterval == 0)
        {
            _serverRepository.Save(saveData).Forget();
            Debug.Log($"[CurrencyManager] {_saveCount} Firebase Save executed");
        }
        else
        {
            _localRepository.Save(saveData).Forget();
            Debug.Log($"[CurrencyManager] {_saveCount} Local Save executed");
        }
    }
}