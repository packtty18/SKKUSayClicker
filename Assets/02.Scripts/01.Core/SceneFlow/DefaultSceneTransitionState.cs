using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultSceneTransitionState : ISceneTransitionState
{
    public void Enter(ESceneType targetScene)
    {
        Debug.Log($"[SceneState] Enter → {targetScene}");
    }

    public void Exit()
    {
        Debug.Log("[SceneState] Exit");
    }
}

