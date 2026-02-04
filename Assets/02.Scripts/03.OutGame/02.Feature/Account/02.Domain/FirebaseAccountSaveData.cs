using Firebase.Firestore;
using System;

[Serializable]
[FirestoreData]
public class FirebaseAccountSaveData
{
    [FirestoreProperty]
    public string Email { get; set; }
    public FirebaseAccountSaveData()
    { }
    public FirebaseAccountSaveData(string email)
    {
        Email = email;
    }
}