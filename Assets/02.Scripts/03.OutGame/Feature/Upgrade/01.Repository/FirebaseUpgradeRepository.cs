using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEditor.Overlays;
using UnityEngine;

public class FirebaseUpgradeRepository : IUpgradeRepository
{
    private const string DOMAIN = "Upgrade";

    private FirebaseAuth _auth => FirebaseService.Auth;
    private FirebaseFirestore _db => FirebaseService.DB;

    public FirebaseUpgradeRepository()
    {

    }
    public async UniTask<SUpgradeSaveData> Load()
    {
        try
        {
            string uid = _auth.CurrentUser.UserId;
            DocumentSnapshot snapshot =
                await _db.Collection(DOMAIN).Document(uid).GetSnapshotAsync();

            FirebaseUpgradeSaveData save = snapshot.ConvertTo<FirebaseUpgradeSaveData>();
            SUpgradeSaveData result = SUpgradeSaveData.Default;
            result.Level = save.Level;
            return result;
        }
        catch (Exception ex)
        {
            SUpgradeSaveData result = SUpgradeSaveData.Default;
            Debug.LogError("업그레이드 로드 실패" + ex.Message);
            return result;
        }
    }

    public async UniTaskVoid Save(SUpgradeSaveData upgrade)
    {
        try
        {
            string uid = _auth.CurrentUser.UserId;
            FirebaseUpgradeSaveData save = new FirebaseUpgradeSaveData(upgrade);
            await _db.Collection(DOMAIN).Document(uid).SetAsync(save);
        }
        catch (Exception e)
        {
            Debug.LogError("업그레이드 저장 실패" + e.Message);
        }
    }
}
