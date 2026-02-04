using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IAccountRepository
{
    UniTask<AccountResult> LogIn(string email, string password);
    UniTask<AccountResult> Register(string email, string password);
}
