using System.Linq;
using System.Text.RegularExpressions;

public class PasswordComplexitySpecification : ISpecification<string>
{
    private static readonly Regex DigitRegex = new(@"\d");
    private static readonly Regex SpecialCharRegex = new(@"[^A-Za-z0-9]");

    private string _errorMessage;
    public string ErrorMessage => _errorMessage;


    public bool IsSatisfiedBy(string value)
    {
        if (!DigitRegex.IsMatch(value))
        {
            _errorMessage = "비밀번호에 숫자를 최소 1개 포함해야 합니다.";
            return false;
        }

        if (!SpecialCharRegex.IsMatch(value))
        {
            _errorMessage = "비밀번호에 특수문자를 최소 1개 포함해야 합니다.";
            return false;
        }

        return true;
    }
}
