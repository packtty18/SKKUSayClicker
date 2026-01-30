public class AccountExistsSpecification : ISpecification<string>
{
    private readonly IAccountRepository _repository;
    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public AccountExistsSpecification(IAccountRepository repository)
    {
        _repository = repository;
    }

    public bool IsSatisfiedBy(string email)
    {
        if (!_repository.Exists(email))
        {
            _errorMessage = "존재하지 않는 계정";
            return false;
        }
        return true;
    }
}