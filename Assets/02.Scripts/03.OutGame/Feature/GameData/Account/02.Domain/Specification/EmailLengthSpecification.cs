//이메일의 최대길이를 체크....굳이?
public class EmailLengthSpecification : ISpecification<string>
{
    private const int MAX_LENGTH = 254;
    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string value)
    {
        if (value.Length > MAX_LENGTH)
        {
            _errorMessage = $"이메일은 {MAX_LENGTH}자를 초과할 수 없음";
            return false;
        }
        return true;
    }
}