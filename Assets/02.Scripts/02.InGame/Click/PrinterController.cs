using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PrinterController : MonoBehaviour, IClickable, IFeedbackOwner
{
    
    [Header("Feedbacks")]
    [SerializeField] private FeedbackPlayer _clickFeedback;
    [SerializeField] private FeedbackPlayer _productFeedback;

    [SerializeField] private float _remainTime;
    [SerializeField] private bool _isProducing;
    public Transform OwnerTransform => transform;

    [Title("Caching")]
    private DataManager _data => DataManager.Instance;
    private float _productionTime;

    [Title("Observer")]
    public event Action OnTimerChanged;
    public event Action OnProductionCompleted;

    private void Awake()
    {
        
    }

    private void Start()
    {
        StartProduction();
    }

    private void Update()
    {
        if (!_isProducing || _data ==null)
            return;

        _remainTime -= _data.GetDataValue(EPrinterData.ValueByTime) * Time.deltaTime;
        OnTimerChanged?.Invoke();

        if (_remainTime <= 0f)
        {
            CompleteProduction();
            _productFeedback.PlayFeedbacks();
        }

    }
    public void OnClick()
    {
        ReduceTime(_data.GetDataValue(EClickData.ManualClickValue));

        bool isCrit = Random.Range(0f, 1f) <= _data.GetDataValue(EClickData.CritRate) ? true : false;
        SFeedbackData data = new SFeedbackData()
        {
            TextValue = isCrit ? _data.GetDataValue(EClickData.ManualClickValue) * _data.GetDataValue(EClickData.CritIncreaesRate) : _data.GetDataValue(EClickData.ManualClickValue),
            TextType = isCrit ? EFloatTextType.PrinterCritical : EFloatTextType.Printer
        };
        _clickFeedback.PlayFeedbacks(data);
        Debug.Log($"{isCrit}");
    }

    public float GetRatio()
    {
        return 1- _remainTime / _productionTime;
    }    

    private void StartProduction()
    {
        _productionTime = _data.GetDataValue(EPrinterData.DefaultProductTime);
        _remainTime = _productionTime;
        _isProducing = true;

        Debug.Log("[Printer] Production Started");
    }

    private void ReduceTime(float power)
    {
        _remainTime -= power;
        _remainTime = Mathf.Max(0f, _remainTime);
    }

    private void CompleteProduction()
    {
        _isProducing = false;

        Debug.Log("[Printer] Production Completed");

        OnProductionCompleted?.Invoke();
        StartProduction();
    }
}
