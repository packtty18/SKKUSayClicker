using Cysharp.Threading.Tasks;

// 로그인 행위에 대한 검증
public class LoginValidator
{
    public ValidationResult ValidateEmail(string email)
    {
        //이메일 형식 검증
        var emailSpec = new EmailSpecification();
        if (!emailSpec.IsSatisfiedBy(email))
        {
            return ValidationResult.Fail(emailSpec.ErrorMessage);
        }

        return ValidationResult.Success();
    }

    public ValidationResult ValidatePassword(string password)
    {
        var validator = new SpecificationValidator<string>()
            .Add(new PasswordSpecification());

        return validator.ValidateFast(password);
    }
}
