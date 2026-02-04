using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IAccountRepository
{
    UniTask<SAccountResult> LogIn(string email, string password);
    UniTask<SAccountResult> Register(string email, string password);
}
