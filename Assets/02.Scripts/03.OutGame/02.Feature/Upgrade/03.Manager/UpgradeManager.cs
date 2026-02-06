
using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class UpgradeManager : GlobalSingleton<UpgradeManager>
{
    public SafeEvent OnDataChanged = new();

    [SerializeField] private UpgradeSpecTableSO _specTable;

    //매니저 내부에서는 Upgrade 조작 가능
    private readonly Dictionary<EUpgradeType, Upgrade> _upgrades = new();
    public List<IReadOnlyUpgrade> Upgrades => _upgrades.Values.ToList<IReadOnlyUpgrade>();

    private IUpgradeRepository _serverRepository;
    private IUpgradeRepository _localRepository;

    protected override void Init()
    {
        _serverRepository = new FirebaseUpgradeRepository();
        _localRepository = new PlayerPrefsUpgradeRepository(AccountManager.Instance.Email);
        InitLoad().Forget();
    }

    private async UniTask InitLoad()
    {
        UpgradeSaveData serverResult = UpgradeSaveData.Default;
        UpgradeSaveData localResult = UpgradeSaveData.Default;

        try
        {
            serverResult = await Load(_serverRepository);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[UpgradeManager] 서버 로드 실패함: {e.Message}");
        }

        try
        {
            localResult = await Load(_localRepository);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[UpgradeManager] 로컬 로드 실패함: {e.Message}");
        }

        UpgradeSaveData result = SelectLatest(serverResult, localResult);
        ApplyData(result);
    }

    private async UniTask<UpgradeSaveData> Load(IUpgradeRepository target)
    {
        UpgradeSaveData targetSaveData = UpgradeSaveData.Default;

        try
        {
            targetSaveData = await target.Load();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[UpgradeManager] 서버 로드 실패함: {e.Message}");
        }


        return targetSaveData;
    }

    private UpgradeSaveData SelectLatest(UpgradeSaveData server,UpgradeSaveData local)
    {
        bool isServerLatest = server.LastSavedAt >= local.LastSavedAt;

        Debug.Log(
            $"[UpgradeManager] {(isServerLatest ? "서버" : "로컬")} 선택. " +
            $"ServerTime: {server.LastSavedAt.ToDateTime():O}, " +
            $"LocalTime: {local.LastSavedAt.ToDateTime():O}"
        );

        return isServerLatest ? server : local;
    }


    private void ApplyData(UpgradeSaveData data)
    {
        int[] levels = data.Levels;
        foreach(var specData in _specTable.Datas)
        {
            if(_upgrades.ContainsKey(specData.Type))
            {
                throw new Exception($"Duplicate Upgrade Type: {specData.Type}");
            }
            _upgrades.Add(specData.Type, new Upgrade(specData, levels[(int)specData.Type]));
        }

        OnDataChanged?.Invoke();
        Debug.Log($"[CurrencyManager] Currency loaded. SavedAt: {data.LastSavedAt}");
    }
    public IReadOnlyUpgrade Get(EUpgradeType type)
    {
        _upgrades.TryGetValue(type, out var upgrade);
        return upgrade;
    }

    public bool CanLevelUp(EUpgradeType type)
    {
        if (!_upgrades.TryGetValue(type, out var upgrade))
            return false;

        if (!upgrade.CanLevelUp())
            return false;

        return CurrencyManager.Instance
            .CanAfford(upgrade.SpecData.CostType, upgrade.Cost);
    }


    [SerializeField] private float _saveDelaySeconds = 0.6f;
    [SerializeField] private int _firebaseSaveInterval = 5;

    private CancellationTokenSource _saveCts;
    private int _saveCount;

    public bool TryLevelUp(EUpgradeType type)
    {
        if (!CanLevelUp(type))
            return false;

        Upgrade upgrade = _upgrades[type];

        if (!CurrencyManager.Instance.TrySpend(upgrade.SpecData.CostType, upgrade.Cost))
            return false;

        if (!upgrade.TryLevelUp())
            return false;

        OnDataChanged?.Invoke();
        RequestSave();   // 🔥 즉시 저장 ❌ → 저장 요청 ⭕
        return true;
    }

    private void RequestSave()
    {
        _saveCts?.Cancel();
        _saveCts?.Dispose();

        _saveCts = new CancellationTokenSource();
        SaveWithDelay(_saveCts.Token).Forget();
    }

    private async UniTaskVoid SaveWithDelay(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(
                TimeSpan.FromSeconds(_saveDelaySeconds),
                cancellationToken: token
            );

            ExecuteSave();
        }
        catch (OperationCanceledException)
        {
            // 정상 취소 (새 Save 요청 발생)
        }
    }

    private void ExecuteSave()
    {
        _saveCount++;

        var saveData = UpgradeSaveData.Default;

        foreach (var kv in _upgrades)
        {
            saveData.Levels[(int)kv.Key] = kv.Value.Level;
        }

        saveData.LastSavedAt = Timestamp.FromDateTime(DateTime.UtcNow);

        if (_saveCount % _firebaseSaveInterval == 0)
        {
            _serverRepository.Save(saveData).Forget();
            Debug.Log($"[UpgradeManager] {_saveCount} Firebase Save executed");
        }
        else
        {
            _localRepository.Save(saveData).Forget();
            Debug.Log($"[UpgradeManager] {_saveCount} Local Save executed");
        }
    }
}
