using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IUpgradeRepository
{
    UniTaskVoid Save(SUpgradeSaveData upgrade);
    UniTask<SUpgradeSaveData> Load();
}
