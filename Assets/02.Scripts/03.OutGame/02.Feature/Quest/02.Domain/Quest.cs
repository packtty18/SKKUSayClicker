using NUnit.Framework;
using UnityEngine;

public class Quest
{
    public QuestDataSO Data { get; }
    public EQuestState State { get; private set; }

    public Quest(QuestDataSO data)
    {
        Data = data;
        State = EQuestState.InProgress;
    }

    public void Evaluate(QuestContext context)
    {
        if (State != EQuestState.InProgress)
            return;

        if (IsConditionMet(context))
        {
            State = EQuestState.Completed;
            Debug.Log($"[Quest] Completed: {Data.QuestName}");
        }
    }

    private bool IsConditionMet(QuestContext context)
    {
        switch (Data.ConditionType)
        {
            case EQuestConditionType.TotalClicks:
                return context.TotalClickCount >= Data.TargetValue;

            case EQuestConditionType.UpgradeLevel:
                return context.GetUpgradeLevel(Data.TargetUpgradeType) >= Data.TargetValue;

            case EQuestConditionType.TotalMoneyEarned:
                return context.TotalMoneyEarned >= Data.TargetValue;
        }

        return false;
    }

    public void Claim()
    {
        if (State != EQuestState.Completed)
            return;

        State = EQuestState.Claimed;
    }
}
