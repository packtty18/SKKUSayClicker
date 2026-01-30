using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Printer : MonoBehaviour, IClickable, IFeedbackOwner
{
    [Header("Feedbacks")]
    [SerializeField] private FeedbackPlayer _clickFeedback;
    [SerializeField] private FeedbackPlayer _productFeedback;

    [Title("State")]
    [SerializeField, ReadOnly] private bool _isProducing;

    public Transform OwnerTransform => transform;

    [Title("Stats")]
    [ShowInInspector, ReadOnly] private IPrinterStatProvider _stats;

    [SerializeField, ReadOnly] private float _productTime;
    [SerializeField, ReadOnly] private float _progress;

    public float Progress => _progress;
    public float ProductTime => _productTime;

    [Title("Observer")]
    public SafeEvent OnProgress = new();
    public SafeEvent OnProductionCompleted = new();

    #region Unity Lifecycle

    private void Start()
    {
        Init(new PrinterStatProvider());
    }

    private void Update()
    {
        if (!_isProducing)
        {
            return;
        }

        IncreaseProgress(_stats.ValueByTime * Time.deltaTime);
    }

    #endregion

    #region Initialization

    public void Init(IPrinterStatProvider stats)
    {
        _stats = stats;
        _productTime = _stats.ProductTime;
        _progress = 0f;

        StartProduction();
    }

    private void StartProduction()
    {
        _isProducing = true;
        Debug.Log("[Printer] Production Started");
    }

    #endregion

    #region Click Handling
    public void OnClick()
    {
        bool isCrit = Random.value <= _stats.CritRate;
        float increase = isCrit
            ? _stats.ManualClick * _stats.CritIncreaseRate
            : _stats.ManualClick;

        IncreaseProgress(increase);
        PlayClickFeedback(isCrit, increase);

        Debug.Log($"[Printer] Click | Crit: {isCrit}, Power: {increase}");
    }

    private void PlayClickFeedback(bool isCrit, float increase)
    {
        SFeedbackData data = new SFeedbackData
        {
            TextValue = increase,
            TextType = isCrit
                ? EFloatTextType.PrinterCritical
                : EFloatTextType.Printer
        };

        _clickFeedback?.PlayFeedbacks(data);
    }

    #endregion
    #region Production Logic
    private void IncreaseProgress(float power)
    {
        _progress += power;

        while (_progress >= _productTime)
        {
            _progress -= _productTime;
            CompleteProduction();
        }

        OnProgress.Invoke();
    }

    private void CompleteProduction()
    {
        Debug.Log("[Printer] Production Completed");

        OnProductionCompleted.Invoke();
        _productFeedback?.PlayFeedbacks();
    }

    #endregion
}
