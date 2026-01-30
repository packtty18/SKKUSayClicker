public class PasswordLengthSpecification : ISpecification<string>
{
    private const int MIN_LENGTH = 7;
    private const int MAX_LENGTH = 20;
    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string value)
    {
        if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
        {
            _errorMessage = $"비밀번호는 {MIN_LENGTH}-{MAX_LENGTH}자 사이여야 함";
            return false;
        }
        return true;
    }
}