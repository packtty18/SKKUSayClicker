using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSpecTableSO", menuName = "SO/UpgradeSpecTableSO")]
public class UpgradeSpecTableSO : ScriptableObject
{
    [SerializeField] public List<UpgradeSpecData> Datas;
}
