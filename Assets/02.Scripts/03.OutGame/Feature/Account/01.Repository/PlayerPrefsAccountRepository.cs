using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerPrefsAccountRepository : IAccountRepository
{
    private const string DOMAIN = "Account";
    private const string PASSWORD = "Password";
    private const string LAST_ACCOUNT = "LastAccountEmail";
    private const string SALT = "Salt";


    public async UniTask<bool> IsExists(string email)
    {
        bool exists = PlayerPrefs.HasKey(
            PlayerPrefsKeyBuilder.GameData(email, DOMAIN, PASSWORD)
        );
        return await new UniTask<bool>(exists);
    }


    public async UniTask<SAccountResult> LogIn(string email, string password)
    {
        var existResult = await IsExists(email);

        if (!existResult)
            return await new UniTask<SAccountResult>(new SAccountResult(false, "Account not found", null));

        string savedHashedPassword =
            PlayerPrefsRepository.GetString(email, DOMAIN, PASSWORD);

        string inputHashedPassword = GetHashedPassword(password);

        if (!string.Equals(savedHashedPassword, inputHashedPassword))
            return await new UniTask<SAccountResult>(new SAccountResult(false, "Password mismatch", null));

        Debug.Log($"[AccountRepo] Login success: {email}");
        return await new UniTask<SAccountResult>(new SAccountResult(true, "Login success", new Account(email, password)));
    }


    public async UniTask<SAccountResult> Register(string email, string password)
    {
        var existResult = await IsExists(email);

        if (!existResult)
            return new SAccountResult(false, "Account already exists", null);

        try
        {
            string hashedPassword = GetHashedPassword(password);

            PlayerPrefsRepository.SetString(
                email,
                DOMAIN,
                PASSWORD,
                hashedPassword
            );

            Debug.Log($"[AccountRepo] Register success: {email}");
            return new SAccountResult(true, "Register success", new Account(email, password));
        }
        catch (Exception e)
        {
            return new SAccountResult(false, e.Message, null);
        }
    }

    //특정 계정 삭제
    public void Delete(string key)
    {
        PlayerPrefsRepository.DeleteUser(key);
        Debug.Log($"[AccountRepo] Account deleted: {key}");
    }

    
    //계정 및 모든 데이터 삭제
    public void DeleteAll()
    {
        PlayerPrefsRepository.ResetAll();
        Debug.LogWarning("[AccountRepo] DeleteAllSave executed");
    }

    private string GetHashedPassword(string target)
    {
        return Crypto.HashPassword(target + SALT);
    }
}
