using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirebaseAccountRepository : IAccountRepository
{
    public SafeEvent<SAccountResult> OnAuthCompleted = new();

    private bool _isInitialized => FirebaseService.IsInitialized;
    private FirebaseAuth _auth;
    private FirebaseFirestore _db;

    public FirebaseAccountRepository()
    {
        _auth = FirebaseService.Auth;
        _db = FirebaseService.DB;
    }

    public void DeleteAll()
    {
        Debug.LogWarning("[Firebase] DeleteAll() is not supported");
    }

    public async UniTask<bool> Exists(string email)
    {
        try
        {
            //db에 해당 계정데이터가 존재하면 true 없으면 false
            var snapshot = await _db
                .Collection(email)
                .GetSnapshotAsync()
                .AsUniTask();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase DB] 로드 실패 : {e}");
            return false;
        }
    }

    public async UniTask<SAccountResult> LogIn(string email, string password)
    {
        try
        {
            Firebase.Auth.AuthResult result = await _auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
            return new SAccountResult(true);
        }
        catch (Exception e)
        {
            return new SAccountResult(false, e.Message);
        }
    }

    public async UniTask<SAccountResult> Register(string email, string password)
    {
        try
        {
            AuthResult result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();
            string uid = result.User.UserId;
            FirebaseUserDocument userDoc = new FirebaseUserDocument
            {
                Account = new FirebaseAccountSaveData(email),
                Currency = new FirebaseCurrencySaveData
                {
                    Currencies = SCurrencySaveData.Default.Currencies
                },
                Upgrade = new FirebaseUpgradeSaveData
                {
                    Level = SUpgradeSaveData.Default.Level
                },
            };

            await _db
            .Collection("Users")
            .Document(uid)
            .SetAsync(userDoc);

            return new SAccountResult(true);
        }
        catch (Exception e)
        {
            return new SAccountResult(false, e.Message);
        }

    }

    public void Logout()
    {
        _auth.SignOut();
    }
}
