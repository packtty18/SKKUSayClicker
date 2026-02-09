#if !UNITY_WEBGL || UNITY_EDITOR
using Cysharp.Threading.Tasks;

using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEngine;

public static class FirebaseService
{
    public static bool IsInitialized { get; private set; }

    public static FirebaseApp App { get; private set; }
    public static FirebaseAuth Auth { get; private set; }
    public static FirebaseFirestore DB { get; private set; }

    public static SafeEvent OnInitialized = new();

    private static UniTask _initTask;
    private static bool _isInitializing;

    //현재 파이어베이스가 연결되어있는지 체크후 초기화
    public static UniTask InitializeAsync()
    {
        if (IsInitialized)
        {
            return UniTask.CompletedTask;
        }


        if (_isInitializing)
        {
            return _initTask;
        }

        _isInitializing = true;
        _initTask = InitializeInternalAsync();

        return _initTask;
    }

    
    private static async UniTask InitializeInternalAsync()
    {
        Debug.Log("[Firebase] Initializing...");

        DependencyStatus status =
            await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

        if (status != DependencyStatus.Available)
        {
            throw new Exception($"Dependency error : {status}");
        }

        App = FirebaseApp.DefaultInstance;
        Auth = FirebaseAuth.DefaultInstance;
        DB = FirebaseFirestore.DefaultInstance;

        IsInitialized = true;
        _isInitializing = false;

        Debug.Log("[Firebase] Initialized");
        OnInitialized?.Invoke();
    }

    //파이어베이스 연결 해제
    public static void Shutdown()
    {
        if (!IsInitialized)
            return;

        Auth?.SignOut();
        App = null;
        Auth = null;
        DB = null;
        IsInitialized = false;

        Debug.Log("[Firebase] Shutdown");
    }
}
#endif