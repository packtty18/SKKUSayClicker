using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetTextTest : MonoBehaviour
{
    void Start()
    {
        GetText().Forget();
    }

    //1. 데이터 가져오기   : Get
    private async UniTask GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com/search?q=url&oq=url&gs_lcrp=EgZjaHJvbWUyDwgAEEUYORiDARixAxiABDINCAEQABiDARixAxiABDINCAIQABiDARixAxiABDINCAMQABiDARixAxiABDIKCAQQABixAxiABDIGCAUQRRg8MgYIBhBFGDwyBggHEEUYPNIBBzk3NWowajeoAgCwAgA&sourceid=chrome&ie=UTF-8");
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
    //2. 데이터 보내기     : Post
    //3. 데이터 수정하기   : Put
    //4. 데이터 삭제하기   : Delete
}
