
using System;

[Serializable]
public struct UpgradeLevelInfo
{
    public int Level;
    public ECurrencyType NextCostType;
    public float NextCost;

    public void Default()
    {
        Level = 0;
        NextCostType = ECurrencyType.Money;
        NextCost = 0;
    }
}
