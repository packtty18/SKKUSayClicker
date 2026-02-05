using System.Text.RegularExpressions;

public class PasswordSpaceSpecification : ISpecification<string>
{
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

        if (WhiteSpaceRegex.IsMatch(password))
        {
            _errorMessage = "비밀번호에 공백을 포함할 수 없습니다.";
            return false;
        }

        return true;
    }
}
