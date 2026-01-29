using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IPrinterStatProvider
{
    float ManualClick { get; }
    float CritRate { get; }
    float CritIncreaseRate { get; }
    float ProductTime { get; }
    float ValueByTime { get; }
}


public class Printer : MonoBehaviour, IClickable, IFeedbackOwner
{
    
    [Header("Feedbacks")]
    [SerializeField] private FeedbackPlayer _clickFeedback;
    [SerializeField] private FeedbackPlayer _productFeedback;

    [SerializeField] private bool _isProducing;
    public Transform OwnerTransform => transform;

   

    [Title("stat")]
    private IPrinterStatProvider _stats;
    private float _productTime;
    private float _progress;
    public float Progress => _progress;
    public float ProductTime => _productTime;

    [Title("Observer")]
    public SafeEvent OnProgress = new();
    public SafeEvent OnProductionCompleted = new();

    public void Init(IPrinterStatProvider stats)
    {
        _stats = stats;
        _productTime = _stats.ProductTime;
        _progress = _productTime;

        StartProduction();
    }

    private void Start()
    {
        Init(new PrinterStatProvider());
    }

    private void Update()
    {
        if (!_isProducing)
            return;

        _progress += _stats.ValueByTime * Time.deltaTime;

        if (_progress >= _productTime)
        {
            CompleteProduction();
            _productFeedback.PlayFeedbacks();
        }

    }
    public void OnClick()
    {
        bool isCrit = Random.Range(0f, 1f) <= _stats.CritRate ? true : false;
        float increase = isCrit ? _stats.ManualClick * _stats.CritIncreaseRate : _stats.ManualClick;

        IncreaseProgress(increase);
        PlayFeedback(isCrit, increase);

        Debug.Log($"{isCrit}");
    }

    private void PlayFeedback(bool isCrit, float increase)
    {
        SFeedbackData data = new SFeedbackData()
        {
            TextValue = increase,
            TextType = isCrit ? EFloatTextType.PrinterCritical : EFloatTextType.Printer
        };
        _clickFeedback.PlayFeedbacks(data);
    }

    private void StartProduction()
    {
        _progress = _productTime;
        _isProducing = true;

        Debug.Log("[Printer] Production Started");
    }

    private void IncreaseProgress(float power)
    {
        _progress+= power;
    }

    private void CompleteProduction()
    {
        _isProducing = false;

        Debug.Log("[Printer] Production Completed");

        OnProductionCompleted?.Invoke();
        StartProduction();
    }
}
