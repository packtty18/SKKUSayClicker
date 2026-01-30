using Sirenix.OdinInspector;
using UnityEngine;

[SerializeField]
public class PrinterStatProvider : IPrinterStatProvider
{
    [ShowInInspector]public float ManualClick => GameConstants.Click.DEFAULT_CLICK_PROGRESS + UpgradeManager.Instance.Get(EUpgradeType.ClickProgress).value;

    [ShowInInspector] public float CritRate => GameConstants.Click.DEFAULT_CRITICAL_CHANCE + UpgradeManager.Instance.Get(EUpgradeType.CriticalChance).value;

    [ShowInInspector] public float CritIncreaseRate => GameConstants.Click.DEFAULT_CRITICAL_MULTIPLIER + UpgradeManager.Instance.Get(EUpgradeType.CriticalMultiplier).value;

    [ShowInInspector] public float ProductTime => GameConstants.Production.DEFAULT_PROGRESS_TARGET - UpgradeManager.Instance.Get(EUpgradeType.ProgressTarget).value;

    [ShowInInspector] public float ValueByTime => GameConstants.Production.DEFAULT_PROGRESS_BY_TIME + UpgradeManager.Instance.Get(EUpgradeType.ProgressByTime).value;
}
