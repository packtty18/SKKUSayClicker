using System;
using UnityEngine;

/// <summary>
/// Handles printer click & production timer.
/// </summary>
public class PrinterController : MonoBehaviour, IClickable
{
    [Header("Reference")]
    [SerializeField] CardpackSpawner _spawner;
    [Header("Production")]
    [SerializeField] private float _productionTime = 30f;

    [Header("Feedbacks")]
    [SerializeField] private IFeedback[] _feedbacks;

    [SerializeField] private float _remainTime;
    [SerializeField] private bool _isProducing;

    public event Action OnTimerChanged;
    public event Action OnProductionCompleted;

    private void Awake()
    {
        _feedbacks = GetComponentsInChildren<IFeedback>(true);
        StartProduction();

        Debug.Log("[Printer] Initialized");
    }

    private void Update()
    {
        if (!_isProducing)
            return;

        _remainTime -= Time.deltaTime;
        OnTimerChanged?.Invoke();
        if (_remainTime <= 0f)
        {
            _spawner.SpawnPack();
            CompleteProduction();
        }
    }

    /// <summary>
    /// Manual click reduces production time.
    /// </summary>
    public void OnClick(SClickInfo info)
    {
        ReduceTime(info.Power);

        foreach (var feedback in _feedbacks)
        {
            feedback.Play(info);
        }

        Debug.Log($"[Printer] Click Reduce : {info.Power}");
    }

    public float GetRatio()
    {
        return _remainTime / _productionTime;
    }    

    private void StartProduction()
    {
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

        // 다음 생산 자동 시작
        StartProduction();
    }
}
