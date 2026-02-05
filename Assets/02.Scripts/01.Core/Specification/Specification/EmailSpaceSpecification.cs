using System.Text.RegularExpressions;
using UnityEngine;

//이메일이 비어있는지 형식이 맞는지 체크함
public class EmailSpaceSpecification : ISpecification<string>
{

    private string _errorMessage;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            _errorMessage = "이메일이 비어있음";
            return false;
        }

        return true;
    }
}