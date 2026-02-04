public class PasswordCommonWordSpecification : ISpecification<string>
{
    private readonly string[] _commonWords = { "password", "admin", "12345", "qwerty", "abc123" };
    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string value)
    {
        var lowerValue = value.ToLower();
        foreach (var word in _commonWords)
        {
            if (lowerValue.Contains(word))
            {
                _errorMessage = "흔한 비밀번호는 사용할 수 없음";
                return false;
            }
        }
        return true;
    }
}