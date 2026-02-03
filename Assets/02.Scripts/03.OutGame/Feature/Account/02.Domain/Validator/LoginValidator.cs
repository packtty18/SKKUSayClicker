//로그인 행위에 대한 검증
public class LoginValidator
{
    private readonly IAccountRepository _repository;

    public LoginValidator(IAccountRepository repository)
    {
        _repository = repository;
    }

    public ValidationResult ValidateEmail(string email)
    {
        var validator = new SpecificationValidator<string>()
            .Add(new EmailSpecification()) 
            .Add(new AccountExistsSpecification(_repository));

        return validator.ValidateFast(email);
    }

    public ValidationResult ValidatePassword(string password)
    {
        var validator = new SpecificationValidator<string>()
            .Add(new PasswordSpecification());

        return validator.ValidateFast(password);
    }
}