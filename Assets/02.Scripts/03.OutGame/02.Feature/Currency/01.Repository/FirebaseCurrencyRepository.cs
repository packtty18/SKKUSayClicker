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
            DocumentSnapshot snapshot =
                await _db.Collection(DOMAIN).Document(uid).GetSnapshotAsync();

            FirebaseCurrencySaveData save = snapshot.ConvertTo<FirebaseCurrencySaveData>();
            CurrencySaveData result = CurrencySaveData.Default;
            result.Currencies = save.Currencies;
            return result;
        }
        catch (Exception ex)
        {
            CurrencySaveData result = CurrencySaveData.Default;
            Debug.LogError("재화 로드 실패" + ex.Message);
            return result;
        }
    }

    public async UniTaskVoid Save(CurrencySaveData saveData)
    {
        try
        {
            string uid = _auth.CurrentUser.UserId;
            FirebaseCurrencySaveData save = new FirebaseCurrencySaveData(saveData);
            await _db.Collection(DOMAIN).Document(uid).SetAsync(save);
        }
        catch(Exception e)
        {
            Debug.LogError("재화 저장 실패" + e.Message);
        }
    }
}
