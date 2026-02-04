
using Cysharp.Threading.Tasks;

public interface ICurrencyRepository
{
    UniTask Save(SCurrencySaveData saveData);
    UniTask<SCurrencySaveData> Load(); 
}