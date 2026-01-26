using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _text;

    [Header("Move")]
    [SerializeField] private float _moveY = 1f;
    [SerializeField] private float _duration = 0.6f;
    [SerializeField] private Ease _moveEase = Ease.OutCubic;

    [Header("Fade")]
    [SerializeField] private Ease _fadeEase = Ease.OutQuad;

    private Tween _tween;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// UI 플로팅 재생
    /// </summary>
    public void Play(float value)
    {
        Debug.Log($"[FloatingText] Play : {value}");

        _tween?.Kill();

        _text.text = value.ToString();
        _text.alpha = 1f;

        _tween = DOTween.Sequence()
            .Append(transform.DOLocalMoveY(_moveY,_duration)
            .SetEase(_moveEase))
            .Join(_text.DOFade(0f, _duration).SetEase(_fadeEase))
            .OnComplete(() =>
            {
                Destroy(gameObject);
                //gameObject.SetActive(false);
            });
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }
}
