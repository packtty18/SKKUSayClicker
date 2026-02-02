using Sirenix.OdinInspector;
using System;
using UnityEngine;

// 로그인 씬에서만 사용
// Account 도메인 관리 (CRUD + Save) : 생성, 조회, 수정, 삭제, 저장
// 외부와의 소통 창구
public class AccountManager : GlobalSingleton<AccountManager>
{
    [SerializeField] private Account _currentAccount;

    public bool IsLogin => _currentAccount.IsSetted;
    public string Email => _currentAccount?.Email ?? string.Empty;

    private IAccountRepository _repository;
    public IAccountRepository Repository => _repository;

    private LoginValidator _loginValidator;
    private RegisterValidator _registerValidator;

    protected override void Init()
    {
        _repository = new LocalAccountRepository();

        _loginValidator = new LoginValidator(_repository);
        _registerValidator = new RegisterValidator(_repository);
    }

    //로그인 시도
    public SAuthResult TryLogin(string email, string password)
    {
        //1. 입력에 대한 검증
        ValidationResult emailResult = _loginValidator.ValidateEmail(email);
        if (!emailResult.IsValid)
        {
            return new SAuthResult(false, emailResult.FirstError, null);
        }

        ValidationResult passwordResult = _loginValidator.ValidatePassword(password);
        if (!passwordResult.IsValid)
        {
            return new SAuthResult(false, passwordResult.FirstError, null);
        }

        // 2. 리포지토리에서의 검증(해당 로그인의 정보가 리포지토리에 존재하는지 여부 등)
        SAuthResult repositoryLogin = _repository.LogIn(email, password);
        _currentAccount = repositoryLogin.Account;
        return repositoryLogin;
    }

    public SAuthResult TryRegister(string email, string password, string passwordConfirm)
    {
        // 1. 입력의 검증
        ValidationResult emailResult = _registerValidator.ValidateEmail(email);
        if (!emailResult.IsValid)
        {
            return new SAuthResult(false, emailResult.FirstError, null);
        }

        ValidationResult passwordResult = _registerValidator.ValidatePassword(password);
        if (!passwordResult.IsValid)
        {
            return new SAuthResult(false, passwordResult.FirstError, null);
        }

        ValidationResult matchResult = _registerValidator.ValidatePasswordMatch(password, passwordConfirm);
        if (!matchResult.IsValid)
        {
            return new SAuthResult(false, matchResult.FirstError, null);
        }

        SAuthResult repositoryRegister = _repository.Register(email, password);
        return repositoryRegister;
    }

    public void Logout()
    {
        _currentAccount = null;
    }

    [Button("모든 계정데이터 삭제")]
    public void DeleteAll()
    {
        _repository.DeleteAll();
        Logout();
    }

    public string GetLastEmail()
    {
        return _repository.GetLastEmail();
    }
}
