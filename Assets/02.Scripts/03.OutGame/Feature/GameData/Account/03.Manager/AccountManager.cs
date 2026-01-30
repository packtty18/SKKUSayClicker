using System.Data.Common;
using UnityEngine;



//로그인 씬에서만 사용
public class AccountManager : LocalSingleton<AccountManager>
{
    private IAccountRepository _repository;

    protected override void Init()
    {
        _repository = new LocalAccountRepository();
    }

    public ELoginResult Login(string id, string password)
    {
        if (!LoginValidator.IsValidEmail(id))
            return ELoginResult.InvalidIdFormat;

        if (!LoginValidator.IsValidPassword(password))
            return ELoginResult.InvalidPassword;

        if (!_repository.Exists(id))
            return ELoginResult.AccountNotFound;

        string savedHash = _repository.LoadPasswordHash(id);

        if (StringEncoder.Hash(password) != savedHash)
            return ELoginResult.InvalidPassword;

        PlayerPrefs.SetString("LastId", id);
        return ELoginResult.Success;
    }

    public ERegisterResult Register(string id, string password, string confirm)
    {
        if (!LoginValidator.IsValidEmail(id))
            return ERegisterResult.InvalidIdFormat;

        if (_repository.Exists(id))
            return ERegisterResult.DuplicatedId;

        if (!LoginValidator.IsValidPassword(password))
            return ERegisterResult.InvalidPassword;

        if (password != confirm)
            return ERegisterResult.PasswordMismatch;

        _repository.Save(id, StringEncoder.Hash(password));
        PlayerPrefs.SetString("LastId", id);

        return ERegisterResult.Success;
    }

    public void DeleteAccount(string id)
    {
        _repository.DeleteSave(id);
    }
}
