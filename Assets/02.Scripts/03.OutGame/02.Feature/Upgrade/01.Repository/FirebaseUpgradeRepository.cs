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

    public async UniTask<UpgradeSaveData> Load()
    {
        try
        {
            string uid = _auth.CurrentUser.UserId;
            var snapshot = await _db.Collection(DOMAIN).Document(uid).GetSnapshotAsync();

            if (!snapshot.Exists)
                return UpgradeSaveData.CreateDefault();

            return snapshot.ConvertTo<UpgradeSaveData>();
        }
        catch (Exception e)
        {
            Debug.LogError("[FirebaseUpgradeRepository] Load Failed: " + e.Message);
            return UpgradeSaveData.CreateDefault();
        }
    }

    public async UniTask Save(UpgradeSaveData upgrade)
    {
        try
        {
            string uid = _auth.CurrentUser.UserId;
            await _db.Collection(DOMAIN).Document(uid).SetAsync(upgrade);
        }
        catch (Exception e)
        {
            Debug.LogError("업그레이드 저장 실패" + e.Message);
        }
    }
}
