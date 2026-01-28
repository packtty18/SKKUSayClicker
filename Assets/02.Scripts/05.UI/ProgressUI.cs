using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private PrinterController _printer;
    [SerializeField] private Slider _slider;

    private void Awake()
    {
        _printer = GetComponentInParent<PrinterController>();
    }
    private void OnEnable()
    {
        _printer.OnTimerChanged += OnChanged;
    }

    private void OnDisable()
    {
        _printer.OnTimerChanged -= OnChanged;
    }
    private void OnChanged()
    {
        _slider.value = _printer.GetRatio();
    }
}
