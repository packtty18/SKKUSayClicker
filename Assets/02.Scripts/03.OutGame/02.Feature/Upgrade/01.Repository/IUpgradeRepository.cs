using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IUpgradeRepository
{
    UniTaskVoid Save(UpgradeSaveData upgrade);
    UniTask<UpgradeSaveData> Load();
}
