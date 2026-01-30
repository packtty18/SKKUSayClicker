using UnityEngine;

public interface IAccountRepository : IRepository
{
    void Save(Account account);
    Account Load(Account account);
    bool Exists(string email);
    Account Get(string email);
}
