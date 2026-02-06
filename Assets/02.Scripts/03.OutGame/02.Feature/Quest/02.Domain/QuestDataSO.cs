using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Quest/QuestData")]
public class QuestDataSO : ScriptableObject
{
    public string Id;                           //ID. 정렬용?
    public string QuestName;                    //이름

    public EQuestConditionType ConditionType;   //조건 타입

    public int TargetValue;                     //목표값
    [ShowIf(nameof(IsUpgradeQuest))]
    public EUpgradeType TargetUpgradeType;      //조건타입 = 업그레이드

    public ECurrencyType RewardType;            //보상타입
    public int RewardAmount;                    //보상량

    private bool IsUpgradeQuest()
    {
        return ConditionType == EQuestConditionType.UpgradeLevel;
    }
}
