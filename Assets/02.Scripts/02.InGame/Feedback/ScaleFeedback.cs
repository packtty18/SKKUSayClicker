using DG.Tweening;
using UnityEngine;

public class ScaleFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private Transform _owner;

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
        _originScale = _owner.localScale;
    }
    public void Play(SClickInfo info)
    {
        Vector3 targetScale = _originScale * _scaleMultiplier;

        _owner.DOKill();
        _owner.localScale = _originScale;

        Sequence seq = DOTween.Sequence();

        seq.Append(
            _owner.DOScale(targetScale, _inDuration)
              .SetEase(_scaleUpEase)
        );

        seq.Append(
            _owner.DOScale(_originScale, _outDuration)
              .SetEase(_scaleDownEase)
        );
    }
}
