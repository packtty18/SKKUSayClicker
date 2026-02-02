
//해당 계정이 이미 리포지토리에 존재하는지 확인
public class AccountDuplicateSpecification : ISpecification<string>
{
    private readonly IAccountRepository _repository;
    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public AccountDuplicateSpecification(IAccountRepository repository)
    {
        _repository = repository;
    }

    public bool IsSatisfiedBy(string email)
    {
        if (_repository.Exists(email))
        {
            _errorMessage = "중복된 계정";
            return false;
        }
        return true;
    }
}