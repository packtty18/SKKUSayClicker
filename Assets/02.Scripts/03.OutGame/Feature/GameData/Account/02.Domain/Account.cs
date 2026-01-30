using System;
using System.Text.RegularExpressions;

//계정에 대한 도메인
public class Account
{
    public readonly string Email;
    public readonly string Password;

    public Account(string email, string password)
    {
        EmailSpecification emailSpec = new EmailSpecification();
        PasswordSpecification passwordSpec = new PasswordSpecification();

        if (!emailSpec.IsSatisfiedBy(email)) throw new ArgumentException(emailSpec.ErrorMessage);
        if (!passwordSpec.IsSatisfiedBy(password)) throw new ArgumentException(passwordSpec.ErrorMessage);

        Email = email;
        Password = password;
    }

}
