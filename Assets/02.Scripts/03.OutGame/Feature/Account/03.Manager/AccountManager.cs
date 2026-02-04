using Cysharp.Threading.Tasks;
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
        _repository = new FirebaseAccountRepository();

        _loginValidator = new LoginValidator(_repository);
        _registerValidator = new RegisterValidator(_repository);
    }

    // 로그인 시도 (비동기)
    public async UniTask<SAccountResult> TryLoginAsync(string email, string password)
    {
        // 1. 입력에 대한 검증 (비동기 - 계정 존재 여부 체크 포함)
        ValidationResult emailResult = await _loginValidator.ValidateEmailAsync(email);
        if (!emailResult.IsValid)
        {
            return new SAccountResult(false, emailResult.FirstError, null);
        }

        ValidationResult passwordResult = _loginValidator.ValidatePassword(password);
        if (!passwordResult.IsValid)
        {
            return new SAccountResult(false, passwordResult.FirstError, null);
        }

        // 2. 리포지토리에서의 검증 (해당 로그인의 정보가 리포지토리에 존재하는지 여부 등)
        SAccountResult repositoryLogin = await _repository.LogIn(email, password);
        _currentAccount = repositoryLogin.Account;
        return repositoryLogin;
    }

    // 회원가입 시도 (비동기)
    public async UniTask<SAccountResult> TryRegisterAsync(string email, string password, string passwordConfirm)
    {
        // 1. 입력의 검증 (비동기 - 중복 체크 포함)
        ValidationResult emailResult = await _registerValidator.ValidateEmailAsync(email);
        if (!emailResult.IsValid)
        {
            return new SAccountResult(false, emailResult.FirstError, null);
        }

        ValidationResult passwordResult = _registerValidator.ValidatePassword(password);
        if (!passwordResult.IsValid)
        {
            return new SAccountResult(false, passwordResult.FirstError, null);
        }

        ValidationResult matchResult = _registerValidator.ValidatePasswordMatch(password, passwordConfirm);
        if (!matchResult.IsValid)
        {
            return new SAccountResult(false, matchResult.FirstError, null);
        }

        SAccountResult repositoryRegister = await _repository.Register(email, password);
        return repositoryRegister;
    }

    public void Logout()
    {
        _currentAccount = null;
    }
}
