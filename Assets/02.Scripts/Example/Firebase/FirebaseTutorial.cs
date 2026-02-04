using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class FirebaseTutorial : MonoBehaviour
{
    private const string DEFAULT_EMAIL = "packtty18@naver.com";
    private const string DEFAULT_PASSWORD = "12341234";
    private const string DEFAULT_DOGNAME = "흰둥이";
    private const int DEFAULT_DOGAGE = 4;

    [SerializeField] private TextMeshProUGUI _progressText;

    private FirebaseApp _app;
    private FirebaseAuth _auth;
    private FirebaseFirestore _db;

    [ShowInInspector] private FirebaseUser CurrentUser => _auth?.CurrentUser;

    private void Start()
    {
        StartAsync().Forget();
    }

    private async UniTaskVoid StartAsync()
    {
        SetText("Init");
        await FirebaseService.InitializeAsync();

        SetText("LogOut");
        LogOut();

        SetText("LogIn");
        await LogInAsync(DEFAULT_EMAIL, DEFAULT_PASSWORD);

        SetText("SaveDogs");
        await SaveDogsAsync(DEFAULT_DOGNAME, DEFAULT_DOGAGE);
    }

    private void SetText(string text)
    {
        _progressText.text = text;
    }

    [Button]
    public void Register(string email, string password)
    {
        RegisterAsync(email, password).Forget();
    }

    private async UniTask RegisterAsync(string email, string password)
    {
        try
        {
            var result = await _auth
                .CreateUserWithEmailAndPasswordAsync(email, password)
                .AsUniTask();

            Debug.Log($"[Firebase Auth] 회원가입 성공 : {result.User.Email}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase Auth] 회원가입 실패: {e}");
        }
    }

    [Button]
    public void LogIn(string email, string password)
    {
        LogInAsync(email, password).Forget();
    }

    private async UniTask LogInAsync(string email, string password)
    {
        try
        {
            var result = await _auth
                .SignInWithEmailAndPasswordAsync(email, password)
                .AsUniTask();

            Debug.Log($"[Firebase Auth] 로그인 성공 : {result.User.Email}");
        }
        catch (FirebaseException e)
        {
            Debug.LogError($"[Firebase Auth] 파이어베이스에 의한 실패 : {e}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase Auth] 로그인 실패 : {e}");
        }
    }

    [Button]
    public void LogOut()
    {
        _auth.SignOut();
        Debug.Log("[Firebase Auth] 로그아웃 실패");
    }

    [Button]
    public void SaveDogs(string name, int age)
    {
        SaveDogsAsync(name, age).Forget();
    }

    private async UniTask SaveDogsAsync(string name, int age)
    {
        try
        {
            var dog = new DogSaveData(name, age);

            await _db
                .Collection("Dogs")
                .Document("user1")
                .SetAsync(dog)
                .AsUniTask();

            Debug.Log("[Firebase DB] 세이브 성공");
        }
        catch(FirebaseException e)
        {
            Debug.LogError($"[Firebase DB] 파이어베이스에 의한 세이브 실패 : {e}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase DB] 기타 세이브 실패 : {e}");
        }
    }

    [Button]
    public void LoadMyDog()
    {
        LoadMyDogAsync().Forget();
    }

    private async UniTask LoadMyDogAsync()
    {
        try
        {
            var snapshot = await _db
                .Collection("Dogs")
                .Document("user1")
                .GetSnapshotAsync()
                .AsUniTask();

            if (!snapshot.Exists)
            {
                Debug.Log("[Firebase DB] 찾지못함");
                return;
            }

            var dog = snapshot.ConvertTo<DogSaveData>();
            Debug.Log($"[Firebase DB] 로드 성공 : {dog.Name} ({dog.Age})");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase DB] 로드 실패 : {e}");
        }
    }

    [Button]
    public void LoadDogs()
    {
        LoadDogsAsync().Forget();
    }

    private async UniTask LoadDogsAsync()
    {
        try
        {
            var snapshots = await _db
                .Collection("Dogs")
                .GetSnapshotAsync()
                .AsUniTask();

            Debug.Log("[Firebase DB] Dogs List ------------------------");

            foreach (var doc in snapshots.Documents)
            {
                var dog = doc.ConvertTo<DogSaveData>();
                Debug.Log($"{dog.Name} ({dog.Age})");
            }

            Debug.Log("[Firebase DB] Load Complete --------------------");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase DB] 로드 실패 : {e}");
        }
    }

    [Button]
    public void DeleteDogs()
    {
        DeleteDogsAsync().Forget();
    }

    private async UniTask DeleteDogsAsync()
    {
        try
        {
            var snapshots = await _db
                .Collection("Dogs")
                .WhereEqualTo("Name", "누렁이")
                .GetSnapshotAsync()
                .AsUniTask();

            if (snapshots.Count == 0)
            {
                Debug.Log("누렁이 없음");
                return;
            }

            foreach (var doc in snapshots.Documents)
            {
                await doc.Reference.DeleteAsync().AsUniTask();
                Debug.Log($"Deleted : {doc.Id}");
            }

            Debug.Log("[Firebase DB] 삭제 완료");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase DB] 삭제 실패 : {e}");
        }
    }
}
