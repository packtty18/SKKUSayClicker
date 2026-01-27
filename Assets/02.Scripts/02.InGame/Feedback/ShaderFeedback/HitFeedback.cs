using DG.Tweening;
using UnityEngine;

/// <summary>
/// Hit feedback using All in 1 Sprite Shader hit effect.
/// </summary>
public class HitFeedback : FeedbackBase
{
    [Header("Target")]
    [SerializeField] private Renderer _renderer;

    [Header("Hit Effect Settings")]
    [SerializeField] private Color _feedbackColor = Color.white;
    [SerializeField] private float _maxBlend = 0.5f;

    [Header("Duration")]
    [SerializeField] private float _blendInDuration = 0.05f;
    [SerializeField] private float _blendOutDuration = 0.1f;

    [Header("Ease")]
    [SerializeField] private Ease _blendInEase = Ease.OutQuad;
    [SerializeField] private Ease _blendOutEase = Ease.OutCubic;

    private static readonly int HIT_COLOR = Shader.PropertyToID("_HitEffectColor");
    private static readonly int HIT_BLEND = Shader.PropertyToID("_HitEffectBlend");

    private MaterialPropertyBlock _mpb;
    private Tween _currentTween;

    private float _currentBlend;

    private void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();

        _mpb = new MaterialPropertyBlock();
    }

    public override void Play(SFeedbackData data)
    {
        _currentTween?.Kill();

        Debug.Log($"[HitFeedback] Play on {_renderer.name}");

        // Set hit color once
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(HIT_COLOR, _feedbackColor);
        _renderer.SetPropertyBlock(_mpb);

        _currentBlend = 0f;
        SetBlend(0f);

        _currentTween = DOTween.Sequence()
            // Blend In: 0 -> Max
            .Append(DOTween.To(
                () => _currentBlend,
                SetBlend,
                _maxBlend,
                _blendInDuration
            ).SetEase(_blendInEase))

            // Blend Out: Max -> 0
            .Append(DOTween.To(
                () => _currentBlend,
                SetBlend,
                0f,
                _blendOutDuration
            ).SetEase(_blendOutEase))

            // Safety
            .OnKill(() => SetBlend(0f));
    }

    private void SetBlend(float value)
    {
        _currentBlend = value;

        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat(HIT_BLEND, value);
        _renderer.SetPropertyBlock(_mpb);
    }

    private void OnDestroy()
    {
        _currentTween?.Kill();
    }
}
