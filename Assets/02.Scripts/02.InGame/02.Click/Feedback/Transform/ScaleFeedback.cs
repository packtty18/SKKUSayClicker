using DG.Tweening;
using UnityEngine;

public class ScaleFeedback : FeedbackBase
{
    [Header("Scale")]
    [SerializeField] private float _scaleMultiplier = 1.2f;
    [SerializeField] private float _inDuration = 0.15f;
    [SerializeField] private float _outDuration = 0.15f;

    [Header("Ease")]
    [SerializeField] private Ease _scaleUpEase = Ease.OutBack;
    [SerializeField] private Ease _scaleDownEase = Ease.InQuad;

    private Vector3 _originScale;

    private void Start()
    {
        _originScale = OwnerTransform.localScale;
    }
    public override void Play(FeedbackData data)
    {
        Vector3 targetScale = _originScale * _scaleMultiplier;

        OwnerTransform.DOKill();
        OwnerTransform.localScale = _originScale;

        Sequence seq = DOTween.Sequence();

        seq.Append(
            OwnerTransform.DOScale(targetScale, _inDuration)
              .SetEase(_scaleUpEase)
        );

        seq.Append(
            OwnerTransform.DOScale(_originScale, _outDuration)
              .SetEase(_scaleDownEase)
        );
    }
}
