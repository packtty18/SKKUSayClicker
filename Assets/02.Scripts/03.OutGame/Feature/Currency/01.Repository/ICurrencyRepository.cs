
using Cysharp.Threading.Tasks;

public interface ICurrencyRepository
{
    UniTaskVoid Save(SCurrencySaveData saveData);
    UniTask<SCurrencySaveData> Load(); 
}