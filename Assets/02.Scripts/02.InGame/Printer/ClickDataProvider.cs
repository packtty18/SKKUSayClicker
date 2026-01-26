using UnityEngine;

public class ClickDataProvider : MonoBehaviour
{
    public SClickInfo CreateManualClick(Vector2 clickPos)
    {
        SClickInfo info = new SClickInfo
        {
            ClickType = EClickType.Manual,
            Power = GameManager.Instance.ManualDamage,
            Position = new Vector2(clickPos.x, clickPos.y)
        };
        return info;
    }

    public SClickInfo CreateAutoClick(Vector2 clickPos)
    {
        return new SClickInfo
        {
            ClickType = EClickType.Auto,
            Power = GameManager.Instance.AutoDamage,
            Position = new Vector2(clickPos.x, clickPos.y)
        };
    }
}
