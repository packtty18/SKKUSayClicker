using UnityEngine;

public interface ISceneTransitionState
{
    void Enter(ESceneType targetScene);
    void Exit();
}