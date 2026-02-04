using Cysharp.Threading.Tasks;

// 비동기 Repository 체크를 위한 Specification
//public class AccountExistsSpecification
//{
//    private readonly IAccountRepository _repository;
//    private string _errorMessage;
//    public string ErrorMessage => _errorMessage;

//    public AccountExistsSpecification(IAccountRepository repository)
//    {
//        _repository = repository;
//    }

//    public async UniTask<bool> IsSatisfiedByAsync(string email)
//    {
//        bool exists = await _repository.IsExists(email);
//        if (!exists)
//        {
//            _errorMessage = "존재하지 않는 계정";
//            return false;
//        }
//        return true;
//    }
//}