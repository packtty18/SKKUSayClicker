using Sirenix.OdinInspector;
using UnityEngine;

//자식들의 Feedback을 캐싱하고 명령이 내려오면 플레이한다
public class FeedbackPlayer : MonoBehaviour
{
    [ShowInInspector] private IFeedback[] _feedbacks;

    private void Awake()
    {
        _feedbacks = GetComponentsInChildren<IFeedback>(true);
    }

    public void PlayFeedbacks(SFeedbackData data = default)
    {
        foreach (var feedback in _feedbacks)
        {
            feedback.Play(data);
        }
    }
}
