using UnityEngine;

public interface IUpgradeRepository : IRepository
{
    public void Save(SUpgradeSaveData upgrade);
    public SUpgradeSaveData Load();
    
}
