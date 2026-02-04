using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : GlobalSingleton<MySceneManager>
{
    [SerializeField] private SceneTableSO _sceneTable;

    private ISceneTransitionState _currentState;

    public event Action<ESceneType> OnSceneLoadStarted;
    public event Action<float> OnSceneLoadProgress;
    public event Action<ESceneType> OnSceneLoadCompleted;

    protected override void Init()
    {
        _currentState = new DefaultSceneTransitionState();
        Debug.Log("[MySceneManager] Initialized");
    }

    public void ChangeScene(ESceneType targetScene)
    {
        string sceneName = _sceneTable.GetSceneName(targetScene);
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"[MySceneManager] Scene name not found for {targetScene}");
            return;
        }

        StartCoroutine(ChangeSceneRoutine(targetScene));
    }

    private IEnumerator ChangeSceneRoutine(ESceneType targetScene)
    {
        // 1. 로딩 시작 알림
        OnSceneLoadStarted?.Invoke(targetScene);

        // 2. Loading 씬을 Additive로 로드 (현재 씬 위에 오버레이)
        string loadingSceneName = _sceneTable.GetSceneName(ESceneType.Loading);
        AsyncOperation loadingAO = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
        
        while (!loadingAO.isDone)
            yield return null;

        Debug.Log("[MySceneManager] Loading scene loaded");

        // 3. LoadingUI가 준비될 시간 확보 (1프레임 대기)
        yield return null;

        // 4. 타겟 씬 Async 로딩
        string targetSceneName = _sceneTable.GetSceneName(targetScene);
        AsyncOperation targetAO = SceneManager.LoadSceneAsync(targetSceneName);
        targetAO.allowSceneActivation = false;

        float fakeProgress = 0f;
        
        // 5. 로딩 진행도 업데이트
        while (!targetAO.isDone)
        {
            // Unity AsyncOperation의 progress는 0.9까지만 올라감
            float realProgress = Mathf.Clamp01(targetAO.progress / 0.9f);
            
            // 부드러운 진행을 위해 Lerp 사용
            fakeProgress = Mathf.MoveTowards(fakeProgress, realProgress, Time.deltaTime * 0.5f);
            
            OnSceneLoadProgress?.Invoke(fakeProgress);

            // 로딩이 거의 완료되면 Scene Activation 허용
            if (targetAO.progress >= 0.9f)
            {
                // 최소 로딩 시간 보장 (선택사항)
                yield return new WaitForSeconds(0.5f);
                
                // 100% 표시
                OnSceneLoadProgress?.Invoke(1f);
                yield return new WaitForSeconds(0.3f);
                
                targetAO.allowSceneActivation = true;
            }
            
            yield return null;
        }
        Debug.Log($"[MySceneManager] Scene loaded: {targetScene}");
        OnSceneLoadCompleted?.Invoke(targetScene);
    }

    public void SetState(ISceneTransitionState state)
    {
        _currentState = state;
    }
}
