using UnityEngine;

public class GameFlowManager : LocalSingleton<GameFlowManager>
{
    protected override void Init()
    {
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    public void GotoTitleScene()
    {
        MySceneManager.Instance.ChangeScene(ESceneType.Title);
    }

}
