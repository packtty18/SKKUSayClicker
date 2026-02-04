using DG.Tweening;
using UnityEngine;

public class UIScaleFeedback : FeedbackBase
{
    [Header("Scale")]
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float scaleUpDuration = 0.15f;
    [SerializeField] private float scaleDownDuration = 0.15f;

    [Header("Ease")]
    [SerializeField] private Ease scaleUpEase = Ease.OutBack;
    [SerializeField] private Ease scaleDownEase = Ease.InQuad;

    private RectTransform _rectTransform;
    private Vector3 _originScale;
    private Sequence _sequence;

    private void Awake()
    {
        _rectTransform = OwnerTransform as RectTransform;

        if (_rectTransform == null)
        {
            Debug.LogError("[UIScaleFeedback] OwnerTransform is not RectTransform.", this);
            enabled = false;
            return;
        }

        _originScale = _rectTransform.localScale;
    }

    public override void Play(SFeedbackData data)
    {
        if (_rectTransform == null)
            return;

        _sequence?.Kill();

        _rectTransform.localScale = _originScale;

        Vector3 targetScale = _originScale * scaleMultiplier;

        _sequence = DOTween.Sequence()
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        _sequence.Append(
            _rectTransform.DOScale(targetScale, scaleUpDuration)
                .SetEase(scaleUpEase)
        );

        _sequence.Append(
            _rectTransform.DOScale(_originScale, scaleDownDuration)
                .SetEase(scaleDownEase)
        );
    }

    private void OnDisable()
    {
        _sequence?.Kill();
        _rectTransform.localScale = _originScale;
    }
}
