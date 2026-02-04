using Cysharp.Threading.Tasks;

// 로그인 행위에 대한 검증
public class LoginValidator
{
    private readonly IAccountRepository _repository;

    public LoginValidator(IAccountRepository repository)
    {
        _repository = repository;
    }

    public async UniTask<ValidationResult> ValidateEmailAsync(string email)
    {
        // 1. 이메일 형식 검증 (동기)
        var emailSpec = new EmailSpecification();
        if (!emailSpec.IsSatisfiedBy(email))
        {
            return ValidationResult.Fail(emailSpec.ErrorMessage);
        }

        // 2. 계정 존재 여부 검증 (비동기)
        //var existsSpec = new AccountExistsSpecification(_repository);
        //bool exists = await existsSpec.IsSatisfiedByAsync(email);
        //if (!exists)
        //{
        //    return ValidationResult.Fail(existsSpec.ErrorMessage);
        //}

        return ValidationResult.Success();
    }

    public ValidationResult ValidatePassword(string password)
    {
        var validator = new SpecificationValidator<string>()
            .Add(new PasswordSpecification());

        return validator.ValidateFast(password);
    }
}
