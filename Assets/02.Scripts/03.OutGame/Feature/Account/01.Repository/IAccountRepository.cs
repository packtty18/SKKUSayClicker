using UnityEngine;

public interface IAccountRepository : IRepository
{
    bool Exists(string email);
    SAuthResult LogIn(string email, string password);
    SAuthResult Register(string email, string password);

    string GetLastEmail();
}
