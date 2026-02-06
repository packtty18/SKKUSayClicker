using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IUpgradeRepository
{
    UniTask Save(UpgradeSaveData upgrade);
    UniTask<UpgradeSaveData> Load();
}
