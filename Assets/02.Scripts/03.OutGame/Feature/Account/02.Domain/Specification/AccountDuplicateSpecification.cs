using Cysharp.Threading.Tasks;

// 비동기 Repository 체크를 위한 Specification
// 해당 계정이 이미 리포지토리에 존재하는지 확인
//public class AccountDuplicateSpecification
//{
//    private readonly IAccountRepository _repository;
//    private string _errorMessage;
//    public string ErrorMessage => _errorMessage;

//    public AccountDuplicateSpecification(IAccountRepository repository)
//    {
//        _repository = repository;
//    }

//    public async UniTask<bool> IsSatisfiedByAsync(string email)
//    {
//        bool exists = await _repository.IsExists(email);
//        if (exists)
//        {
//            _errorMessage = "중복된 계정";
//            return false;
//        }
//        return true;
//    }
//}
