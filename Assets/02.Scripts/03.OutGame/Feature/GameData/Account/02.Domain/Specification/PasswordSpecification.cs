using System.Text.RegularExpressions;
using UnityEngine;

 public class PasswordSpecification : ISpecification<string>
{
    private readonly Regex _passwordRegex =
        new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^A-Za-z0-9])(?!.*\d)\S{7,20}$");

    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string password)
    {
        if(string.IsNullOrEmpty(password))
        {
            _errorMessage = "비밀번호가 비어있음";
            return false;
        }

        if(!_passwordRegex.IsMatch(password))
        {
            _errorMessage = "비밀번호가 형식에 맞지 않음";
            return false;
        }

        return true;
    }
}