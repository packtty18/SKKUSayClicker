using Firebase.Firestore;
using Sirenix.OdinInspector;
using System;

//계정에 대한 도메인
[Serializable]
public class Account
{
    [ShowInInspector, ReadOnly] public readonly string Email;
    //보안성에 따라 비밀번호는 검증만 시행하고 클라이언트 환경에서 저장하지 않음
    //[ShowInInspector, ReadOnly] public readonly string Password;
    [ShowInInspector, ReadOnly] public readonly bool IsSetted = false;
    public Account(string email, string password)
    {
        EmailSpaceSpecification emailSpec = new EmailSpaceSpecification();
        PasswordSpaceSpecification passwordSpec = new PasswordSpaceSpecification();

        if (!emailSpec.IsSatisfiedBy(email)) throw new ArgumentException(emailSpec.ErrorMessage);
        if (!passwordSpec.IsSatisfiedBy(password)) throw new ArgumentException(passwordSpec.ErrorMessage);

        Email = email;
        //Password = password;
        IsSetted = true;
    }
}