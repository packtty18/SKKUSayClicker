using UnityEngine;

public class GameFlowManager : LocalSingleton<GameFlowManager>
{
    protected override void Init()
    {
    }

    public void GotoTitleScene()
    {
        MySceneManager.Instance.ChangeScene(ESceneType.Title);
    }

}
