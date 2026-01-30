using UnityEngine;

//저장과 불러오기 만을 담당(검증 없음)
public class LocalAccountRepository : IAccountRepository
{    
    public void Save(Account account)
    {
        PlayerPrefs.SetString(account.Email, account.Password);
        PlayerPrefs.Save();
    }

    public Account Load(Account account)
    {
        if (!Exists(account.Email))
            return null;

        string password = PlayerPrefs.GetString(account.Email);
        return new Account(account.Email, password);
    }

    public bool Exists(string email)
    {
        return PlayerPrefs.HasKey(email);
    }

    public Account Get(string email)
    {
        if (!Exists(email))
            return null;

        string password = PlayerPrefs.GetString(email);
        return new Account(email, password);
    }

    public void DeleteAllSave()
    {
        PlayerPrefs.DeleteAll();
    }
}
