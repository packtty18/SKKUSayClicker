using DG.Tweening;
using Lean.Pool;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour, IPlayFeedback, IFloaterText
{
    [SerializeField] private TextMeshPro _text;

    [Header("Move")]
    [SerializeField] private float _moveY = 1f;
    [SerializeField] private float _duration = 0.6f;
    [SerializeField] private Ease _moveEase = Ease.OutCubic;

    [Header("Fade")]
    [SerializeField] private Ease _fadeEase = Ease.OutQuad;

    private Tween _tween;

    public void SetFloater(SFloaterTextContext context)
    {
        SetType(context.Type);
        SetText(context.Value);
    }
    public void Play()
    {
        _tween?.Kill();
        _text.alpha = 1f;
        _tween = DOTween.Sequence()
            .Append(transform.DOLocalMoveY(_moveY, _duration)
            .SetEase(_moveEase))
            .Join(_text.DOFade(0f, _duration).SetEase(_fadeEase))
            .OnComplete(() =>
            {
                LeanPool.Despawn(this);
                //Destroy(gameObject);
                //gameObject.SetActive(false);
            });
    }

    private void SetText(double value)
    {
        _text.text += Utils.FormattedString(value);
    }

    private void SetType(EFloatTextType type)
    {
        switch (type)
        {
            case EFloatTextType.Printer: //아이콘 없은 텍스트 하얀색
                _text.text = "<sprite=0>";
                _text.color = Color.white;
                break;
            case EFloatTextType.PrinterCritical:  //아이콘 없음 텍스트 빨강
                _text.text = "<sprite=3>";
                _text.color = Color.red;
                break;
            case EFloatTextType.Money:  //돈아이콘 , 텍스트 초록
                _text.text = "<sprite=1>";
                _text.color = Color.green;
                break;
            case EFloatTextType.Prestigy: //명성아이콘, 텍스트 노랑
                _text.text = "<sprite=2>";
                _text.color = Color.yellow;
                break;
        }
    }
}
