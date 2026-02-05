//비밀번호에 대한 검증
public class PasswordValidator
{
    private readonly SpecificationValidator<string> _validator;

    public PasswordValidator()
    {
        _validator = new SpecificationValidator<string>()
            .Add(new PasswordSpecification())               //주어진 형식에 맞는가
            .Add(new PasswordLengthSpecification())         //길이가 적절한가
            .Add(new PasswordComplexitySpecification())     //충분히 복잡한가
            .Add(new PasswordCommonWordSpecification());    //흔한 이름인가(블랙리스트랑 동일)
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
