public class QuestContext
{
    public int TotalClickCount;         
    public int TotalMoneyEarned;

    public int GetUpgradeLevel(EUpgradeType type)
    {
        return UpgradeManager.Instance.Get(type).Level;
    }
}