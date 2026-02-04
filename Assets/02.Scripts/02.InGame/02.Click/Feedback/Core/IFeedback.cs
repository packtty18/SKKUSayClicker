using UnityEngine;

public interface IFeedback
{
    Transform OwnerTransform { get; }
    void Play(FeedbackData data = default);
}
