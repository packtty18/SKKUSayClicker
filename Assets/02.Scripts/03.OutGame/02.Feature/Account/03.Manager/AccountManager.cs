using Cysharp.Threading.Tasks;
using NUnit.Framework.Constraints;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

// 로그인 씬에서만 사용
// Account 도메인 관리 (CRUD + Save) : 생성, 조회, 수정, 삭제, 저장
// 외부와의 소통 창구
public class AccountManager : GlobalSingleton<AccountManager>
{
    [SerializeField,ReadOnly] private string _currentEmail;

    public bool IsLogin => !string.IsNullOrEmpty(_currentEmail);
    public string Email => _currentEmail;

    private IAccountRepository _repository;
    public IAccountRepository Repository => _repository;

    private LoginValidator _loginValidator;
    private RegisterValidator _registerValidator;

    protected override void Init()
    {
        _repository = new FirebaseAccountRepository();

        _loginValidator = new LoginValidator();
        _registerValidator = new RegisterValidator();
    }

    

    // 로그인 시도 (비동기)
    public async UniTask<AccountResult> TryLoginAsync(string email, string password)
    {
        AccountResult repositoryLogin = await _repository.LogIn(email, password);
        _currentEmail = repositoryLogin.Email;
        return repositoryLogin;
    }

    // 회원가입 시도 (비동기)
    public async UniTask<AccountResult> TryRegisterAsync(string email, string password, string passwordConfirm)
    {
        AccountResult repositoryRegister = await _repository.Register(email, password);
        return repositoryRegister;
    }

    public void Logout()
    {
        _currentEmail = string.Empty;
    }

    public ValidationResult ValidateLoginInput(string email, string password)
    {
        ValidationResult emailResult = _loginValidator.ValidateEmail(email);
        if (!emailResult.IsValid)
            return emailResult;

        ValidationResult passwordResult = _loginValidator.ValidatePassword(password);
        if (!passwordResult.IsValid)
            return passwordResult;

        return ValidationResult.Success();
    }

    public ValidationResult ValidateRegisterInput(string email, string password, string confirmPassword)
    {
        ValidationResult emailResult = _registerValidator.ValidateEmail(email);
        if (!emailResult.IsValid)
            return emailResult;

        ValidationResult passwordResult = _registerValidator.ValidatePassword(password);
        if (!passwordResult.IsValid)
            return passwordResult;

        ValidationResult matchResult =
            _registerValidator.ValidatePasswordMatch(password, confirmPassword);
        if (!matchResult.IsValid)
            return matchResult;

        return ValidationResult.Success();
    }
}
