using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private Printer _printer;
    [SerializeField] private Slider _slider;

    private void Awake()
    {
        _printer = GetComponentInParent<Printer>();
    }
    private void Start()
    {
        _printer.OnProgress.Subscribe(OnChanged);
    }

    private void OnDisable()
    {
        _printer.OnProgress.Unsubscribe(OnChanged);
    }


    private void OnChanged()
    {
        _slider.value = _printer.Progress / _printer.ProductTime;
    }
}
