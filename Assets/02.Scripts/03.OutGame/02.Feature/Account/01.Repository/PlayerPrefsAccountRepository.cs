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


    public async UniTask<AccountResult> LogIn(string email, string password)
    {
        var existResult = await IsExists(email);

        if (!existResult)
            return await new UniTask<AccountResult>(new AccountResult(false, "Account not found", null));

        string savedHashedPassword =
            UserScopedPlayerPrefs.GetString(email, DOMAIN, PASSWORD);

        string inputHashedPassword = GetHashedPassword(password);

        if (!string.Equals(savedHashedPassword, inputHashedPassword))
            return await new UniTask<AccountResult>(new AccountResult(false, "Password mismatch", null));

        Debug.Log($"[AccountRepo] Login success: {email}");
        return await new UniTask<AccountResult>(new AccountResult(true, "Login success", email));
    }


    public async UniTask<AccountResult> Register(string email, string password)
    {
        var existResult = await IsExists(email);

        if (!existResult)
            return new AccountResult(false, "Account already exists", null);

        try
        {
            string hashedPassword = GetHashedPassword(password);

            UserScopedPlayerPrefs.SetString(
                email,
                DOMAIN,
                PASSWORD,
                hashedPassword
            );

            Debug.Log($"[AccountRepo] Register success: {email}");
            return new AccountResult(true, "Register success", email);
        }
        catch (Exception e)
        {
            return new AccountResult(false, e.Message, null);
        }
    }

    private string GetHashedPassword(string target)
    {
        return Crypto.HashPassword(target + SALT);
    }
}
