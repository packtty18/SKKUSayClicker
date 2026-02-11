using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonStatSlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;

    [Header("Color Settings")]
    [SerializeField] private Gradient statColorGradient;
    [SerializeField] private bool useDefaultGradient = true;

    private int _currentValue;
    private int _maxValue;

    private void Awake()
    {
        if (useDefaultGradient)
        {
            SetupDefaultGradient();
        }
    }

    /// <summary>
    /// 스탯 정보를 설정합니다
    /// </summary>
    /// <param name="statName">스탯 이름 (예: HP, Attack)</param>
    /// <param name="value">현재 스탯 값</param>
    /// <param name="maxValue">최대 스탯 값 (기본: 255)</param>
    public void SetStat(string statName, int value, int maxValue = 255)
    {
        _currentValue = Mathf.Clamp(value, 0, maxValue);
        _maxValue = maxValue;

        // 스탯 이름 설정
        if (statNameText != null)
        {
            statNameText.text = statName;
        }

        // 스탯 값 텍스트 설정
        if (statValueText != null)
        {
            statValueText.text = $"{_currentValue}/{_maxValue}";
        }

        // 슬라이더 설정
        if (slider != null)
        {
            slider.maxValue = maxValue;
            slider.value = _currentValue;
        }

        // 슬라이더 색상 설정
        UpdateSliderColor();
    }

    /// <summary>
    /// 슬라이더 색상을 스탯 값에 따라 업데이트합니다
    /// </summary>
    private void UpdateSliderColor()
    {
        if (fillImage == null || statColorGradient == null) return;

        float normalizedValue = _maxValue > 0 ? (float)_currentValue / _maxValue : 0f;
        fillImage.color = statColorGradient.Evaluate(normalizedValue);
    }

    /// <summary>
    /// 기본 그라데이션 설정 (낮은 값: 빨강, 중간 값: 노랑, 높은 값: 초록)
    /// </summary>
    private void SetupDefaultGradient()
    {
        statColorGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[5];
        colorKeys[0] = new GradientColorKey(new Color(0.8f, 0.2f, 0.2f), 0.0f);   // 빨강 (0%)
        colorKeys[1] = new GradientColorKey(new Color(0.9f, 0.5f, 0.2f), 0.25f);  // 주황 (25%)
        colorKeys[2] = new GradientColorKey(new Color(0.9f, 0.9f, 0.2f), 0.5f);   // 노랑 (50%)
        colorKeys[3] = new GradientColorKey(new Color(0.5f, 0.8f, 0.3f), 0.75f);  // 연두 (75%)
        colorKeys[4] = new GradientColorKey(new Color(0.2f, 0.8f, 0.2f), 1.0f);   // 초록 (100%)

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphaKeys[1] = new GradientAlphaKey(1.0f, 1.0f);

        statColorGradient.SetKeys(colorKeys, alphaKeys);
    }

    /// <summary>
    /// 현재 스탯 값을 반환합니다
    /// </summary>
    public int GetCurrentValue() => _currentValue;

    /// <summary>
    /// 최대 스탯 값을 반환합니다
    /// </summary>
    public int GetMaxValue() => _maxValue;

    /// <summary>
    /// 정규화된 값 (0.0 ~ 1.0)을 반환합니다
    /// </summary>
    public float GetNormalizedValue()
    {
        return _maxValue > 0 ? (float)_currentValue / _maxValue : 0f;
    }
}