using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirebaseAccountRepository : IAccountRepository
{
    private const string DOMAIN = "Account";
    public SafeEvent<AccountResult> OnAuthCompleted = new();

    private bool _isInitialized => FirebaseService.IsInitialized;
    private FirebaseAuth _auth => FirebaseService.Auth;
    private FirebaseFirestore _db => FirebaseService.DB;

    public FirebaseAccountRepository()
    {
    }

    public void DeleteAll()
    {
        Debug.LogWarning("[Firebase] DeleteAll() is not supported");
    }

    public async UniTask<AccountResult> LogIn(string email, string password)
    {
        try
        {
            AuthResult result = await _auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
            return new AccountResult(true,"", email);
        }
        catch (Exception e)
        {
            return new AccountResult(false, e.Message);
        }
    }

    public async UniTask<AccountResult> Register(string email, string password)
    {
        try
        {
            //계정 생성
            AuthResult result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();
            string uid = result.User.UserId;

            //계정 생성시 계정 데이터 초기 생성
            var batch = _db.StartBatch();

            batch.Set(
                _db.Collection("Account").Document(uid),
                new FirebaseAccountSaveData(email)
            );

            batch.Set(
                _db.Collection("Currency").Document(uid),
                new FirebaseCurrencySaveData(CurrencySaveData.Default)
            );

            batch.Set(
                _db.Collection("Upgrade").Document(uid),
                new FirebaseUpgradeSaveData(UpgradeSaveData.Default)
            );
            await batch.CommitAsync();

            return new AccountResult(true, "성공", email);
        }
        catch (Exception e)
        {
            if (_auth.CurrentUser != null)
                await _auth.CurrentUser.DeleteAsync();
            return new AccountResult(false, e.Message);
        }

    }

    public void Logout()
    {
        _auth.SignOut();
    }
}
