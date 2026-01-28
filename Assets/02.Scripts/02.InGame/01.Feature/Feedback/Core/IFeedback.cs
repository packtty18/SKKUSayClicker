using UnityEngine;

public interface IFeedback
{
    Transform OwnerTransform { get; }
    void Play(SFeedbackData data = default);
}
