//회원가입 행위에 대한 검증

public class RegisterValidator
{
    private readonly IAccountRepository _repository;

    public RegisterValidator(IAccountRepository repository)
    {
        _repository = repository;
    }

    public ValidationResult ValidateEmail(string email)
    {
        var emailValidator = new EmailValidator(_repository);
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
            return new ValidationResult(false, new System.Collections.Generic.List<string> { matchSpec.ErrorMessage });
        }
        return new ValidationResult(true, new System.Collections.Generic.List<string>());
    }
}