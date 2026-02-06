using System.Collections.Generic;
using UnityEngine;

public class QuestManager : GlobalSingleton<QuestManager>
{
    public SafeEvent OnQuestUpdated = new();

    [SerializeField] private List<QuestDataSO> _questTable;

    private readonly List<Quest> _quests = new();
    private readonly QuestContext _context = new();

    protected override void Init()
    {
        foreach (var data in _questTable)
        {
            _quests.Add(new Quest(data));
        }

        BindEvents();
    }

    private void BindEvents()
    {
        ClickInputHandler.Instance.OnClick.Subscribe(OnClicked);
        //CurrencyManager.Instance.OnDataChanged += OnCurrencyChanged;
        //UpgradeManager.Instance.OnDataChanged += OnUpgradeChanged;
    }

    private void OnClicked()
    {
        _context.TotalClickCount++;
        EvaluateAll();
    }

    private void OnCurrencyChanged(ECurrencyType type)
    {
        if (type == ECurrencyType.Money)
        {
            _context.TotalMoneyEarned++;
            EvaluateAll();
        }
    }

    private void OnUpgradeChanged()
    {
        EvaluateAll();
    }

    private void EvaluateAll()
    {
        foreach (var quest in _quests)
        {
            quest.Evaluate(_context);
        }

        OnQuestUpdated?.Invoke();
    }
    public void ClaimReward(string questId)
    {
        Quest quest = _quests.Find(q => q.Data.Id == questId);

        if (quest == null || quest.State != EQuestState.Completed)
            return;

        CurrencyManager.Instance.Add(
            quest.Data.RewardType,
            quest.Data.RewardAmount
        );

        quest.Claim();

        Debug.Log($"[Quest] Reward claimed: {quest.Data.QuestName}");
        OnQuestUpdated?.Invoke();
    }
}