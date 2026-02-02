//이메일에 대한 검증
public class EmailValidator
{
    private readonly SpecificationValidator<string> _validator;

    public EmailValidator(IAccountRepository repository)
    {
        _validator = new SpecificationValidator<string>()
            .Add(new EmailSpecification())
            .Add(new EmailLengthSpecification())
            .Add(new EmailDomainSpecification())
            .Add(new EmailBlacklistSpecification())
            .Add(new AccountDuplicateSpecification(repository));
    }

    public ValidationResult Validate(string email)
    {
        return _validator.ValidateFast(email);
    }

    public ValidationResult ValidateAll(string email)
    {
        return _validator.Validate(email);
    }
}