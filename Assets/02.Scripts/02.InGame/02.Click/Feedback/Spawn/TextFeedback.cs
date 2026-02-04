using UnityEngine;

public class TextFeedback : SpawnFeedback
{
    [SerializeField] private EFloatTextType _type;

    protected override void CheckInterface(GameObject target, FeedbackData data)
    {
        if (target.TryGetComponent<IFloaterText>(out var floater))
        {
            SFloaterTextContext context = new SFloaterTextContext()
            {
                Value = data.TextValue,
                Type = data.TextType
            };

            floater.SetFloater(context);
        }

        base.CheckInterface(target, data);
    }
}
