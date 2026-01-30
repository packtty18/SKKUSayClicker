//이것이 이메일의 도메인인지 체크
public class EmailDomainSpecification : ISpecification<string>
{
    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string value)
    {
        var parts = value.Split('@');
        if (parts.Length != 2 || parts[1].Split('.').Length < 2)
        {
            _errorMessage = "유효하지 않은 도메인";
            return false;
        }
        return true;
    }
}