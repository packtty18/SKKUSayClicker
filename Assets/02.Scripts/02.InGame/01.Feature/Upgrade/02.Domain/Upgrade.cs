using UnityEngine;

public class Upgrade
{
    public readonly UpgradeSpecData SpecData;
    public int Level { get; private set; }
    public SCurrency Cost => SpecData.BaseCost + Mathf.Pow(SpecData.CostMultiplier, Level);   // 지수공식
    public float value => SpecData.BaseValue + Level * SpecData.ValueMultiplier;          //선형공식
    public bool IsMaxLevel => Level >= SpecData.MaxLevel;


    public Upgrade(UpgradeSpecData specData)
    {

        if(specData.MaxLevel < 0)
        {
            throw new System.ArgumentException("최대 레벨이 0보다 작음");
        }

        if (specData.BaseCost < 0)
        {
            throw new System.ArgumentException("기본 코스트가 0보다 작음");
        }
        if (specData.BaseValue < 0)
        {
            throw new System.ArgumentException("기본 값이 0보다 작음");
        }
        if (specData.CostMultiplier < 0)
        {
            throw new System.ArgumentException("코스트 증가율이 0보다 작음");
        }
        if (specData.ValueMultiplier < 0)
        {
            throw new System.ArgumentException("값 증가율이 0보다 작음");
        }
        if (string.IsNullOrEmpty(specData.Name))
        {
            throw new System.ArgumentException("이름이 비어있음");
        }
        if (string.IsNullOrEmpty(specData.Description))
        {
            throw new System.ArgumentException("설명이 비어있음");
        }

        SpecData = specData;
    }

    public bool TryLevelUp()
    {
        if(!IsMaxLevel)
        {
            return false;
        }

        Level++;
        return true;
    }
}
