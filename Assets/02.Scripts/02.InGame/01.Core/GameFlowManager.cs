using System;
using UnityEngine;

public enum InGameState
{
    None,
    Initializing,
    Playing,
    Pause
}

public class GameFlowManager : LocalSingleton<GameFlowManager>
{
    public event Action<InGameState> OnStateChanged;

    public InGameState CurrentState { get; private set; } = InGameState.None;

    protected override void Init()
    {
        Debug.Log("[GameFlowManager] Init");
        ChangeState(InGameState.Initializing);
    }

    public void StartGame()
    {
        Debug.Log("[GameFlowManager] Start Game");
        ChangeState(InGameState.Playing);
    }

    public void PauseGame()
    {
        if (CurrentState != InGameState.Playing)
            return;

        Time.timeScale = 0f;
        ChangeState(InGameState.Pause);
    }

    public void ResumeGame()
    {
        if (CurrentState != InGameState.Pause)
            return;

        Time.timeScale = 1f;
        ChangeState(InGameState.Playing);
    }

    public void ExitToTitle()
    {
        Time.timeScale = 1f;
        MySceneManager.Instance.ChangeScene(ESceneType.Title);
    }

    private void ChangeState(InGameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        Debug.Log($"[GameFlowManager] State Changed: {newState}");
        OnStateChanged?.Invoke(newState);
    }
}
