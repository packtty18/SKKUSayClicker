using Cysharp.Threading.Tasks;

// 회원가입 행위에 대한 검증
public class RegisterValidator
{
    public RegisterValidator()
    {
    }

    public ValidationResult ValidateEmail(string email)
    {
        var emailValidator = new EmailValidator();
        return emailValidator.Validate(email);
    }

    public ValidationResult ValidatePassword(string password)
    {
        var passwordValidator = new PasswordValidator();
        return passwordValidator.Validate(password);
    }

    public ValidationResult ValidatePasswordMatch(string password, string passwordConfirm)
    {
        var matchSpec = new PasswordMatchSpecification();
        if (!matchSpec.IsSatisfiedBy((password, passwordConfirm)))
        {
            return ValidationResult.Fail(matchSpec.ErrorMessage);
        }
        return ValidationResult.Success();
    }
}
