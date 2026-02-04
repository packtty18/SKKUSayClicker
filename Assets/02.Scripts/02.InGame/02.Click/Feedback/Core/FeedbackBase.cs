using Sirenix.OdinInspector;
using UnityEngine;

public abstract class FeedbackBase : MonoBehaviour, IFeedback
{
    [ShowInInspector] protected IFeedbackOwner _owner;
    public Transform OwnerTransform => _owner.OwnerTransform;

    private void Awake()
    {
        _owner = GetComponentInParent<IFeedbackOwner>();
    }

    public abstract void Play(SFeedbackData data);
}
