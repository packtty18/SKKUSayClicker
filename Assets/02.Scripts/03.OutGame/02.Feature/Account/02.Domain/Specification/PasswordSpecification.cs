using System.Text.RegularExpressions;

public class PasswordSpecification : ISpecification<string>
{
    private const int MinLength = 7;
    private const int MaxLength = 20;

    private static readonly Regex DigitRegex = new(@"\d");
    private static readonly Regex SpecialCharRegex = new(@"[^A-Za-z0-9]");
    private static readonly Regex WhiteSpaceRegex = new(@"\s");

    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            _errorMessage = "비밀번호가 비어있습니다.";
            return false;
        }

        if (password.Length < MinLength || password.Length > MaxLength)
        {
            _errorMessage = $"비밀번호는 {MinLength}~{MaxLength}자여야 합니다.";
            return false;
        }

        if (WhiteSpaceRegex.IsMatch(password))
        {
            _errorMessage = "비밀번호에 공백을 포함할 수 없습니다.";
            return false;
        }

        if (!DigitRegex.IsMatch(password))
        {
            _errorMessage = "비밀번호에 숫자를 최소 1개 포함해야 합니다.";
            return false;
        }

        if (!SpecialCharRegex.IsMatch(password))
        {
            _errorMessage = "비밀번호에 특수문자를 최소 1개 포함해야 합니다.";
            return false;
        }

        return true;
    }
}
