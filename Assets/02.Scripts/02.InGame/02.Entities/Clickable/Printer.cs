using Sirenix.OdinInspector;
using System;
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
    private RuntimeValue<float> _progress;
    public RuntimeValue<float> Progress => _progress;

    [Title("Observer")]
    public event Action OnProductionCompleted;

    public void Init(IPrinterStatProvider stats)
    {
        _stats = stats;

        _progress = new RuntimeValue<float>(
            _stats.ProductTime,
            0,
            _stats.ValueByTime
        );

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

        _progress.Regeneration(Time.deltaTime);

        if (_progress.IsFull())
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
        _progress.Reset();
        _isProducing = true;

        Debug.Log("[Printer] Production Started");
    }

    private void IncreaseProgress(float power)
    {
        _progress.Increase(power);
    }

    private void CompleteProduction()
    {
        _isProducing = false;

        Debug.Log("[Printer] Production Completed");

        OnProductionCompleted?.Invoke();
        StartProduction();
    }
}
