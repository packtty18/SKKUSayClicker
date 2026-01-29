using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class CurrencyUI : MonoBehaviour
{
    private CurrencyManager _data => CurrencyManager.Instance;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private ECurrencyType _targetData;

    [SerializeField] private UnityEvent _onChangeEvent;

    private void Start()
    {
        CurrencyManager.OnDataChanged.Subscribe(OnChanged);
        ResetUI();
    }

    private void OnDestroy()
    {
        CurrencyManager.OnDataChanged.Unsubscribe(OnChanged);
    }

    private void OnChanged(ECurrencyType type)
    {
        if(type != _targetData)
        {
            return;
        }
        _text.text = Utils.FormattedString(_target.Value);

        _onChangeEvent?.Invoke();
    }

    private void ResetUI()
    {
        _text.text = Utils.FormattedString(_target.Value);
    }
}