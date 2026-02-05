using Cysharp.Threading.Tasks;

// 이메일에 대한 검증
public class EmailValidator
{
    private readonly SpecificationValidator<string> _syncValidator;

    public EmailValidator()
    {
        // 동기 검증만 포함
        _syncValidator = new SpecificationValidator<string>()
            .Add(new EmailSpaceSpecification())
            .Add(new EmailLengthSpecification())
            .Add(new EmailDomainSpecification())
            .Add(new EmailBlacklistSpecification());
    }

    // 동기 검증만 수행
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
