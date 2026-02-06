using Firebase.Firestore;
using System;

[Serializable]
[FirestoreData]
public class AccountSaveData
{
    [FirestoreProperty]
    public string Email { get; set; }
    public AccountSaveData()
    { }
    public AccountSaveData(string email)
    {
        Email = email;
    }
}