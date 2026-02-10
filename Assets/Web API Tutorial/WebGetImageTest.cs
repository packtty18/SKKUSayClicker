using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebGetImageTest : MonoBehaviour
{
    [SerializeField] private RawImage _image;
    void Start()
    {
        GetTexture().Forget();
    }

    private async UniTask GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://yt3.googleusercontent.com/2GbJoy1rf88ByUwmy1Kc05BcnxH33wbjAxRdqg2n6_VSoZsKTbVKrvPs3zivavdHbuTIC5iV=s900-c-k-c0x00ffffff-no-rj");
        await www.SendWebRequest();

        Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        _image.texture = myTexture; 
    }
}
