using UnityEngine;
using UnityEngine.EventSystems;
using MoreMountains.Feedbacks;

public class UIHoverFeedback : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("Feel")]
    [SerializeField] private MMF_Player hoverEnterFeedback;
    [SerializeField] private MMF_Player hoverExitFeedback;

    private bool _isHovered;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isHovered)
            return;

        _isHovered = true;

        hoverExitFeedback?.StopFeedbacks();
        hoverEnterFeedback?.PlayFeedbacks();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isHovered)
            return;

        _isHovered = false;

        hoverEnterFeedback?.StopFeedbacks();
        hoverExitFeedback?.PlayFeedbacks();
    }
}
