
//현재 이메일이 블랙리스트에 등록되어있는지 체크
public class EmailBlacklistSpecification : ISpecification<string>
{

    //이것또한 리포지토리에서 읽어오면 좋을듯. 크게 다룰것 같지는 않으니 패스
    private readonly string[] _blacklist = { "tempmail.com", "guerrillamail.com", "10minutemail.com" };
    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string value)
    {
        foreach (var blocked in _blacklist)
        {
            if (value.EndsWith($"@{blocked}"))
            {
                _errorMessage = "차단된 이메일 도메인";
                return false;
            }
        }
        return true;
    }
}