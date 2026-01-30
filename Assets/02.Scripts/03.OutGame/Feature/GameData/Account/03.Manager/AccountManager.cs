using System;
using UnityEngine;

// 로그인 씬에서만 사용
// Account 도메인 관리 (CRUD + Save) : 생성, 조회, 수정, 삭제, 저장
// 외부와의 소통 창구
public class AccountManager : GloblaManager<AccountManager>
{
    private const string LAST_ACCOUNT = "LastAccountEmail";
   
    [SerializeField] private Account _currentAccount = null;
    public bool IsLogin => _currentAccount != null;
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
        // 1. 이메일 검증 (형식 + 존재 여부)
        ValidationResult emailResult = _loginValidator.ValidateEmail(email);
        if (!emailResult.IsValid)
        {
            return new SAuthResult(false, emailResult.FirstError, null);
        }

        // 2. 비밀번호 형식 검증
        ValidationResult passwordResult = _loginValidator.ValidatePassword(password);
        if (!passwordResult.IsValid)
        {
            return new SAuthResult(false, passwordResult.FirstError, null);
        }

        // 3. 저장된 계정과 비밀번호 일치 확인
        Account savedAccount = _repository.Get(email);
        if (savedAccount.Password != password)
        {
            return new SAuthResult(false, "패스워드 실패", null);
        }

        _currentAccount = savedAccount;
        return new SAuthResult(true, "로그인 성공", savedAccount);
    }

    public SAuthResult TryRegister(string email, string password, string passwordConfirm)
    {
        // 1. 이메일 검증
        ValidationResult emailResult = _registerValidator.ValidateEmail(email);
        if (!emailResult.IsValid)
        {
            return new SAuthResult(false, emailResult.FirstError, null);
        }

        // 2. 비밀번호 검증
        ValidationResult passwordResult = _registerValidator.ValidatePassword(password);
        if (!passwordResult.IsValid)
        {
            return new SAuthResult(false, passwordResult.FirstError, null);
        }

        // 3. 비밀번호 확인 검증
        ValidationResult matchResult = _registerValidator.ValidatePasswordMatch(password, passwordConfirm);
        if (!matchResult.IsValid)
        {
            return new SAuthResult(false, matchResult.FirstError, null);
        }

        // 4. Account 생성 (생성자에서 추가 검증)
        Account account;
        try
        {
            account = new Account(email, password);
        }
        catch (Exception e)
        {
            return new SAuthResult(false, e.Message, null);
        }

        // 5. 저장
        _repository.Save(account);
        PlayerPrefs.SetString(LAST_ACCOUNT, email);
        PlayerPrefs.Save();

        // 6. 회원가입 성공
        _currentAccount = account;
        return new SAuthResult(true, "회원가입 성공", account);
    }

    public void Logout()
    {
        _currentAccount = null;
    }

    public string GetLastAccountEmail()
    {
        return PlayerPrefs.GetString(LAST_ACCOUNT, string.Empty);
    }
}
