using Cysharp.Threading.Tasks;

// 이메일에 대한 검증
public class EmailValidator
{
    private readonly IAccountRepository _repository;
    private readonly SpecificationValidator<string> _syncValidator;

    public EmailValidator(IAccountRepository repository)
    {
        _repository = repository;
        
        // 동기 검증만 포함
        _syncValidator = new SpecificationValidator<string>()
            .Add(new EmailSpecification())
            .Add(new EmailLengthSpecification())
            .Add(new EmailDomainSpecification())
            .Add(new EmailBlacklistSpecification());
    }

    // 비동기 검증 (중복 체크 포함)
    public async UniTask<ValidationResult> ValidateAsync(string email)
    {
        // 1. 동기 검증 먼저 수행
        ValidationResult syncResult = _syncValidator.ValidateFast(email);
        if (!syncResult.IsValid)
        {
            return syncResult;
        }

        // 2. 비동기 중복 검증
        var duplicateSpec = new AccountDuplicateSpecification(_repository);
        bool isNotDuplicate = await duplicateSpec.IsSatisfiedByAsync(email);
        if (!isNotDuplicate)
        {
            return ValidationResult.Fail(duplicateSpec.ErrorMessage);
        }

        return ValidationResult.Success();
    }

    // 동기 검증만 수행 (중복 체크 제외)
    public ValidationResult Validate(string email)
    {
        return _syncValidator.ValidateFast(email);
    }

    // 모든 동기 검증 수행
    public ValidationResult ValidateAll(string email)
    {
        return _syncValidator.Validate(email);
    }
}
