using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private Printer _printer;
    [SerializeField] private Slider _slider;

    private RuntimeValue<float> _value => _printer.Progress;
    private void Awake()
    {
        _printer = GetComponentInParent<Printer>();
    }
    private void Start()
    {
        _value.Subscribe(OnChanged);
    }

    private void OnDisable()
    {
        _value.Unsubscribe(OnChanged);
    }


    private void OnChanged(float current)
    {
        _slider.value = _value.GetRatio();
    }
}
