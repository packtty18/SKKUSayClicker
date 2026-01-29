using UnityEngine;

public interface ICurrencyRepository : IRepository
{
    public void Save(SCurrencySaveData saveData);
    public SCurrencySaveData Load();
}
