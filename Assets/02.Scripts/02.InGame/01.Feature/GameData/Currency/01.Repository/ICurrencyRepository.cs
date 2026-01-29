using UnityEngine;

public interface ICurrencyRepository 
{
    public void Save(SCurrencySaveData saveData);
    public SCurrencySaveData Load();
}
