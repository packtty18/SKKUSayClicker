using Cysharp.Threading.Tasks;

// 로그인 행위에 대한 검증
public class LoginValidator
{
    public ValidationResult ValidateEmail(string email)
    {
        var validator = new EmailValidator();

        return validator.Validate(email);
    }

    public ValidationResult ValidatePassword(string password)
    {
        var validator = new PasswordValidator();

        return validator.Validate(password);
    }
}
