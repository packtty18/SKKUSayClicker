using AllIn1VfxToolkit.Demo.Scripts;
using UnityEngine;

public class ShakeFeedback : FeedbackBase
{
    [SerializeField] private float shakeAmount = 0.15f;

    public override void Play(FeedbackData data)
    {
        if (AllIn1Shaker.i != null)
        {
            AllIn1Shaker.i.DoCameraShake(shakeAmount);
        }

        else Debug.LogError($"No AllIn1Shaker found. Please add one to the scene");
    }
}
