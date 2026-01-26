using UnityEngine;

//인게임 자체의 게임 진행
public class GameManager : LocalSingleton<GameManager>
{
    public float ManualDamage = 1;
    public float AutoDamage = 0.1f;

    protected override void Init()
    {
        
    }
}
