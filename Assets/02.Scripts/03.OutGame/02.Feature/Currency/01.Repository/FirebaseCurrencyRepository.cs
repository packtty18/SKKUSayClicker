using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using System.Threading.Tasks;
using UnityEditor.Overlays;
using UnityEngine;

public class FirebaseCurrencyRepository : ICurrencyRepository
{
    private const string DOMAIN = "Currency";

    private FirebaseAuth _auth => FirebaseService.Auth;
    private FirebaseFirestore _db => FirebaseService.DB;

    public FirebaseCurrencyRepository()
    {
       
    }

    public async UniTask<CurrencySaveData> Load()
    {
        try
        {
            string uid = _auth.CurrentUser.UserId;
            var snapshot = await _db.Collection(DOMAIN).Document(uid).GetSnapshotAsync();

            if (!snapshot.Exists)
                return CurrencySaveData.CreateDefault();

            return snapshot.ConvertTo<CurrencySaveData>();
        }
        catch (Exception e)
        {
            Debug.LogError("[FirebaseCurrencyRepository] Load Failed: " + e.Message);
            return CurrencySaveData.CreateDefault();
        }
    }


    public async UniTask Save(CurrencySaveData saveData)
    {
        try
        {
            string uid = _auth.CurrentUser.UserId;
            await _db.Collection(DOMAIN).Document(uid).SetAsync(saveData);
        }
        catch(Exception e)
        {
            Debug.LogError("재화 저장 실패" + e.Message);
        }
    }
}
