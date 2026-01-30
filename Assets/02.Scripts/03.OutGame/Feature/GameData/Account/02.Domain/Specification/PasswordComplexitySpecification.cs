using System.Linq;

public class PasswordComplexitySpecification : ISpecification<string>
{
    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string value)
    {
        bool hasLower = value.Any(char.IsLower);
        bool hasUpper = value.Any(char.IsUpper);
        bool hasSpecial = value.Any(c => !char.IsLetterOrDigit(c));

        if (!hasLower || !hasUpper || !hasSpecial)
        {
            _errorMessage = "비밀번호는 대문자, 소문자, 특수문자를 포함해야 함";
            return false;
        }
        return true;
    }
}
