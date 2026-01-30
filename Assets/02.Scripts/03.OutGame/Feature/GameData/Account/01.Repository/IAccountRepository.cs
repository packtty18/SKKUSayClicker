using UnityEngine;

public interface IAccountRepository : IRepository
{
    bool Exists(string id);
    string LoadPasswordHash(string id);
    void Save(string id, string passwordHash);

    void DeleteSave(string id);

}
