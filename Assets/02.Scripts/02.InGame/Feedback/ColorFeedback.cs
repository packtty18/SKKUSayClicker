using DG.Tweening;
using UnityEngine;

public class ColorFeedback : MonoBehaviour, IFeedback
{
    [Header("Target")]
    [SerializeField] private Renderer _renderer;

    [Header("Color Settings")]
    [SerializeField] private Color _feedbackColor = Color.white;
    [SerializeField] private float _colorInDuration = 0.05f;
    [SerializeField] private float _colorOutDuration = 0.1f;

    [Header("Ease")]
    [SerializeField] private Ease _colorInEase = Ease.OutQuad;
    [SerializeField] private Ease _colorOutEase = Ease.OutCubic;

    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    private MaterialPropertyBlock _mpb;
    private Color _originalColor;
    private Tween _currentTween;

    private void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();

        _mpb = new MaterialPropertyBlock();

        // 원본 컬러 캐싱 (Shared Material 기준)
        if (_renderer.sharedMaterial.HasProperty(ColorProperty))
            _originalColor = _renderer.sharedMaterial.GetColor(ColorProperty);
        else
            Debug.LogWarning($"[ColorFeedback] {_renderer.name} has no _Color property");
    }

    public void Play(SClickInfo info)
    {
        _currentTween?.Kill();

        Debug.Log($"[ColorFeedback] Play on {_renderer.name}");

        _currentTween = DOTween.Sequence()
            .Append(DOTween.To(
                () => _originalColor,
                c => ApplyColor(c),
                _feedbackColor,
                _colorInDuration
            ).SetEase(_colorInEase))
            .Append(DOTween.To(
                () => _feedbackColor,
                c => ApplyColor(c),
                _originalColor,
                _colorOutDuration
            ).SetEase(_colorOutEase));
    }

    private void ApplyColor(Color color)
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(ColorProperty, color);
        _renderer.SetPropertyBlock(_mpb);
    }

    private void OnDestroy()
    {
        _currentTween?.Kill();
    }
}
