
using Sirenix.OdinInspector;
using UnityEngine;

//전체 게임의 실행을 담당
public class GameManager : GlobalSingleton<GameManager>
{
    

    protected override void Init()
    {
    }

    [Button]
    public void OnApplicationQuit()
    {
    }
}
