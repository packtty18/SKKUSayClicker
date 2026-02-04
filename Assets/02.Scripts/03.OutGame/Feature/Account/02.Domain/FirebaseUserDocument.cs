using Firebase.Firestore;
using UnityEngine;

public class FirebaseUserDocument
{
    [FirestoreProperty] public FirebaseAccountSaveData Account { get; set; }
    [FirestoreProperty] public FirebaseCurrencySaveData Currency { get; set; }
    [FirestoreProperty] public FirebaseUpgradeSaveData Upgrade { get; set; }
}