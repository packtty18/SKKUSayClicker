using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseCurrencyRepository : ICurrencyRepository
{
    public SafeEvent<SAccountResult> OnAuthCompleted = new();
    private FirebaseFirestore _db;

    public FirebaseCurrencyRepository()
    {
        _db = FirebaseService.DB;
    }

    public UniTask<SCurrencySaveData> Load()
    {
        throw new System.NotImplementedException();
    }

    public UniTask Save(SCurrencySaveData saveData)
    {
        throw new System.NotImplementedException();
    }
}
