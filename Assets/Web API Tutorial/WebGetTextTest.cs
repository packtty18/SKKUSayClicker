using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetTextTest : MonoBehaviour
{
    private async void Start()
    {
        string result = await GetWebText("https://www.google.com/search?q=url&oq=url&gs_lcrp=EgZjaHJvbWUyDwgAEEUYORiDARixAxiABDINCAEQABiDARixAxiABDINCAIQABiDARixAxiABDINCAMQABiDARixAxiABDIKCAQQABixAxiABDIGCAUQRRg8MgYIBhBFGDwyBggHEEUYPNIBBzk3NWowajeoAgCwAgA&sourceid=chrome&ie=UTF-8");
        Debug.Log(result);
    }

    //1. 데이터 가져오기   : Get
    private async UniTask<string> GetWebText(string url)
    {
        var text = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        return text;
    }
    //2. 데이터 보내기     : Post
    //3. 데이터 수정하기   : Put
    //4. 데이터 삭제하기   : Delete
}
