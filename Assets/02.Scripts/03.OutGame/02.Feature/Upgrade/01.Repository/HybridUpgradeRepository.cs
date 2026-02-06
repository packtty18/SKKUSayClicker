using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class HybridUpgradeRepository : IUpgradeRepository
{
    private readonly IUpgradeRepository _local;
    private readonly IUpgradeRepository _server;

    private float _saveDelaySeconds = 0.6f;
    private int _firebaseSaveInterval = 5;

    private CancellationTokenSource _saveCts;
    private int _saveCount;

    public HybridUpgradeRepository()
    {
        _server = new FirebaseUpgradeRepository();
        _local = new PlayerPrefsUpgradeRepository(AccountManager.Instance.Email);
    }

    public async UniTask<UpgradeSaveData> Load()
    {
        UpgradeSaveData local = await _local.Load();
        UpgradeSaveData server = await _server.Load();

        UpgradeSaveData selected = SelectLatest(server, local);

        return selected;
    }

    private UpgradeSaveData SelectLatest(UpgradeSaveData server, UpgradeSaveData local)
    {
        bool isServerLatest = server.LastSavedAt >= local.LastSavedAt;

        Debug.Log(
            $"[HybridUpgradeRepository] {(isServerLatest ? "서버" : "로컬")} 선택. " +
            $"ServerTime: {server.LastSavedAt.ToDateTime():O}, " +
            $"LocalTime: {local.LastSavedAt.ToDateTime():O}"
        );

        return isServerLatest ? server : local;
    }



    public UniTask Save(UpgradeSaveData saveData)
    {
        _saveCts?.Cancel();     //이전 예약이 있다면 취소
        _saveCts?.Dispose();    //완전히 리소스를 해제. 이후 

        _saveCts = new CancellationTokenSource();

        //해당 토큰을 사용하는 비동기 함수 실행
        SaveWithDelay(saveData, _saveCts.Token).Forget();
        return UniTask.CompletedTask;
    }


    private async UniTaskVoid SaveWithDelay(UpgradeSaveData saveData, CancellationToken token)
    {
        //도중에 토큰이 Cancel 되면 catch 실행.
        try
        {
            //세이브 대기시간 동안 대기 + Cancellation 토큰을 등록함
            await UniTask.Delay(
                TimeSpan.FromSeconds(_saveDelaySeconds)
                , cancellationToken: token);

            ExecuteSave(saveData);
        }
        catch (OperationCanceledException)
        {
            // 정상적인 취소 (새 Save 요청 들어옴)
            // 아직 취소로직은 없음 그냥 세이브 안함
        }
    }
    private void ExecuteSave(UpgradeSaveData saveData)
    {
        _saveCount++;

        if (_saveCount % _firebaseSaveInterval == 0)
        {
            _server.Save(saveData).Forget();
            Debug.Log($"[HybridUpgradeRepository] {_saveCount} Firebase Save executed");
        }
        else
        {
            _local.Save(saveData).Forget();
            Debug.Log($"[HybridUpgradeRepository] {_saveCount} Local Save executed");
        }
    }
}
