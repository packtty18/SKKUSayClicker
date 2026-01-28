using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CurrencyUI : MonoBehaviour
{
    private DataManager _data => DataManager.Instance;
    [SerializeField] private DataValue<float> _target;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private UnityEvent _onChangeEvent;

    private void OnEnable()
    {
        _target.Subscribe(OnChanged);
    }

    private void OnDisable()
    {
        _target.Unsubscribe(OnChanged);
    }
    private void OnChanged(float value)
    {
        _text.text = Utils.FormattedString(value);

        _onChangeEvent?.Invoke();
    }
}