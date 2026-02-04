using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IUpgradeRepository
{
    void Save(SUpgradeSaveData upgrade);
    SUpgradeSaveData Load();
}
