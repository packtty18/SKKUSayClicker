//비밀번호에 대한 검증
public class PasswordValidator
{
    private readonly SpecificationValidator<string> _validator;

    public PasswordValidator()
    {
        _validator = new SpecificationValidator<string>()
            .Add(new PasswordSpecification())
            .Add(new PasswordLengthSpecification())
            .Add(new PasswordComplexitySpecification())
            .Add(new PasswordCommonWordSpecification());
    }

    public ValidationResult Validate(string password)
    {
        return _validator.ValidateFast(password);
    }

    public ValidationResult ValidateAll(string password)
    {
        return _validator.Validate(password);
    }
}
