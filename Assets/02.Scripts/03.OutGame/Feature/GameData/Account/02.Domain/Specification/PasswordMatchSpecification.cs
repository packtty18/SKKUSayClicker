public class PasswordMatchSpecification : ISpecification<(string password, string confirmPassword)>
{
    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy((string password, string confirmPassword) value)
    {
        if (string.IsNullOrEmpty(value.password) || string.IsNullOrEmpty(value.confirmPassword))
        {
            _errorMessage = "비밀번호가 비어있음";
            return false;
        }
        if (!string.Equals(value.password, value.confirmPassword))
        {
            _errorMessage = "비밀번호와 비밀번호 확인이 다름";
            return false;
        }
        return true;
    }
}