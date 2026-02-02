using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Slider _progressBar;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Awake()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        if (_progressBar != null)
        {
            _progressBar.value = 0f;
        }

        if (_progressText != null)
        {
            _progressText.text = "Loading... 0%";
        }

        // 이벤트 구독
        if (MySceneManager.Instance != null)
        {
            MySceneManager.Instance.OnSceneLoadStarted += OnLoadStarted;
            MySceneManager.Instance.OnSceneLoadProgress += OnLoadProgress;
            MySceneManager.Instance.OnSceneLoadCompleted += OnLoadCompleted;
        }
        else
        {
            Debug.LogError("[LoadingUI] MySceneManager.Instance is null!");
        }
    }

    private void OnDestroy()
    {
        if (MySceneManager.Instance == null)
        {
            return;
        }

        MySceneManager.Instance.OnSceneLoadStarted -= OnLoadStarted;
        MySceneManager.Instance.OnSceneLoadProgress -= OnLoadProgress;
        MySceneManager.Instance.OnSceneLoadCompleted -= OnLoadCompleted;
    }

    private void OnLoadStarted(ESceneType scene)
    {
        Debug.Log($"[LoadingUI] Load Started: {scene}");

        if (_progressBar != null)
        {
            _progressBar.value = 0f;
        }

        if (_progressText != null)
        {
            _progressText.text = "Loading... 0%";
        }

        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 1f;
        }
        
        gameObject.SetActive(true);
    }

    private void OnLoadProgress(float progress)
    {
        // 진행도 업데이트
        if (_progressBar != null)
        {
            _progressBar.value = progress;
        }
        
        if (_progressText != null)
        {
            int percentage = Mathf.RoundToInt(progress * 100);
            _progressText.text = $"Loading... {percentage}%";
        }
        
        Debug.Log($"[LoadingUI] Progress: {progress:F2} ({Mathf.RoundToInt(progress * 100)}%)");
    }

    private void OnLoadCompleted(ESceneType scene)
    {
        Debug.Log($"[LoadingUI] Load Completed: {scene}");

        if (_progressBar != null)
        {
            _progressBar.value = 1f;
        }

        if (_progressText != null)
        {
            _progressText.text = "Loading... 100%";
        }

        // 페이드 아웃 효과 (선택사항)
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0f;
        }
    }
}
