using Sirenix.OdinInspector;
using System;

//계정에 대한 도메인
[Serializable]
public class Account
{
    [ShowInInspector, ReadOnly] public readonly string Email;
    //보안성에 따라 비밀번호는 검증만 시행하고 클라이언트 환경에서 저장하지 않음
    //[ShowInInspector, ReadOnly] public readonly string Password;
    public Account(string email, string password)
    {
        EmailValidator emailSpec = new EmailValidator();
        PasswordValidator passwordSpec = new PasswordValidator();

        ValidationResult emailResult = emailSpec.Validate(email);
        ValidationResult passwordResult = passwordSpec.Validate(password);

        if (!emailResult.IsValid) throw new ArgumentException(emailResult.FirstError);
        if (!passwordResult.IsValid) throw new ArgumentException(passwordResult.FirstError);

        Email = email;
    }
}