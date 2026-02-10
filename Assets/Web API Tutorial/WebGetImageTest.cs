using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebGetImageTest : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RawImage _image;
    [SerializeField] private TextMeshProUGUI _text;

    private bool _isLoading;

    [SerializeField] private string IMAGE_URL = "https://placedog.net/500/500?random";

    private void Start()
    {
        LoadImage().Forget();
    }

    [Button]
    public void Refresh()
    {
        if (_isLoading)
        {
            Debug.Log("[WebGetImageTest] Refresh ignored. Already loading.");
            return;
        }

        LoadImage().Forget();
    }

    private async UniTask LoadImage()
    {
        _isLoading = true;
        SetStatus("Loading...");

        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(IMAGE_URL);

        try
        {
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[WebGetImageTest] Error: {request.error}");
                SetStatus($"Error: {request.error}");
                return;
            }

            Texture texture = DownloadHandlerTexture.GetContent(request);
            _image.texture = texture;

            SetStatus("Success!");
            Debug.Log("[WebGetImageTest] Image loaded successfully.");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void SetStatus(string message)
    {
        if (_text != null)
        {
            _text.text = message;
        }
    }
}
