using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class UpgradeSpecData
{
    public EUpgradeType Type;
    public int MaxLevel;
    public ECurrencyType CostType;
    public float BaseCost;
    public float BaseValue;
    public float CostMultiplier;
    public float ValueMultiplier;

    //UI Only
    public string Name;
    [TextArea]
    public string Description;
    [PreviewField]
    public Sprite Icon;


    public bool IsValid(out string errorMessage)
    {
        if (MaxLevel <= 0)
        {
            errorMessage = $"[{Type}] MaxLevel은 0보다 커야 합니다. (현재: {MaxLevel})";
            return false;
        }
        if (BaseCost < 0)
        {
            errorMessage = $"[{Type}] BaseCost는 0 이상이어야 합니다. (현재: {BaseCost})";
            return false;
        }
        if (BaseValue < 0)
        {
            errorMessage = $"[{Type}] BaseValue는 0 이상이어야 합니다. (현재: {BaseValue})";
            return false;
        }

        if (CostMultiplier <= 0)
        {
            errorMessage = $"[{Type}] CostMultiplier는 0보다 커야 합니다. (현재: {CostMultiplier})";
            return false;
        }

        if (ValueMultiplier <= 0)
        {
            errorMessage = $"[{Type}] ValueMultiplier는 0보다 커야 합니다. (현재: {ValueMultiplier})";
            return false;
        }

        if(string.IsNullOrEmpty(Name))
        {
            errorMessage = $"[{Type}] 이름이 비어있습니다. (현재: {Name})";
            return false;
        }

        if (string.IsNullOrEmpty(Description))
        {
            errorMessage = $"[{Type}] 설명이 비어있습니다. (현재: {Description})";
            return false;
        }

        if (Icon == null)
        {
            errorMessage = $"[{Type}] 아이콘이 비어있습니다";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}
