using UnityEngine;

public class Inventory : LocalSingleton<Inventory>
{
    public int PackCount { get; private set; }

    protected override void Init()
    {


    }


    public void AddPack()
    {
        PackCount++;
        Debug.Log($"[Inventory] Pack Count = {PackCount}");
    }

    
}
