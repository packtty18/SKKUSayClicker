using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class CurrencyUI : MonoBehaviour
{
    private DataManager _data => DataManager.Instance;
    [SerializeField] private DataValue<float> _target;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private ECurrentcyData _targetData;

    [SerializeField] private UnityEvent _onChangeEvent;

    private void Start()
    {
        _target = _data.GetData(_targetData);
        _target.Subscribe(OnChanged);
        ResetUI();
    }

    private void OnDestroy()
    {
        _target.Unsubscribe(OnChanged);
    }

    private void OnChanged(float value)
    {
        _text.text = Utils.FormattedString(value);

        _onChangeEvent?.Invoke();
    }

    private void ResetUI()
    {
        _text.text = Utils.FormattedString(_target.Value);
    }
}