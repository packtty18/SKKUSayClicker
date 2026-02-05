//이것이 이메일의 도메인인지 체크
using System.Text.RegularExpressions;

public class EmailDomainSpecification : ISpecification<string>
{
    private readonly Regex _emailRegex = new(@"^[\w.-]+@[\w.-]+\.\w+$");


    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string value)
    {
        if (!_emailRegex.IsMatch(value))
        {
            _errorMessage = "유효하지 않은 도메인";
            return false;
        }
        return true;
    }
}