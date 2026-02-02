using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

// Repository-level validation only
public class LocalAccountRepository : IAccountRepository
{
    private const string DOMAIN = "Account";
    private const string PASSWORD = "Password";
    private const string LAST_ACCOUNT = "LastAccountEmail";
    private const string SALT = "Salt";


    public bool Exists(string email)
    {
        bool exists = PlayerPrefs.HasKey(
            PlayerPrefsKeyBuilder.GameData(email, DOMAIN, PASSWORD)
        );

        return exists;
    }


    public SAuthResult LogIn(string email, string password)
    {
        if (!Exists(email))
            return new SAuthResult(false, "Account not found", null);

        string savedHashedPassword =
            PlayerPrefsRepository.GetString(email, DOMAIN, PASSWORD);

        string inputHashedPassword = GetHashedPassword(password);

        if (!string.Equals(savedHashedPassword, inputHashedPassword))
            return new SAuthResult(false, "Password mismatch", null);

        SaveLastEmail(email);

        Debug.Log($"[AccountRepo] Login success: {email}");
        return new SAuthResult(true, "Login success", new Account(email, password));
    }


    public SAuthResult Register(string email, string password)
    {
        if (Exists(email))
            return new SAuthResult(false, "Account already exists", null);

        try
        {
            string hashedPassword = GetHashedPassword(password);

            PlayerPrefsRepository.SetString(
                email,
                DOMAIN,
                PASSWORD,
                hashedPassword
            );

            SaveLastEmail(email);

            Debug.Log($"[AccountRepo] Register success: {email}");
            return new SAuthResult(true, "Register success", new Account(email, password));
        }
        catch (Exception e)
        {
            return new SAuthResult(false, e.Message, null);
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

    public string GetLastEmail()
    {
        return PlayerPrefsRepository.GetString("Global", DOMAIN, LAST_ACCOUNT, "");
    }

    private void SaveLastEmail(string email)
    {
        PlayerPrefsRepository.SetString("Global", DOMAIN, LAST_ACCOUNT, email);
    }


    private string GetHashedPassword(string target)
    {
        return Crypto.HashPassword(target + SALT);
    }
}
