using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class CurrencyUI : MonoBehaviour
{
    private CurrencyManager _data => CurrencyManager.Instance;
    [SerializeField] private SCurrency _target;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private ECurrencyType _targetData;

    [SerializeField] private UnityEvent _onChangeEvent;

    private void Start()
    {
        _target = _data.Get(_targetData);
        //도메인에서는 이벤트가 있어서는 안되고 매니저를 통해야만 한다.
        //CurrencyManager를 따로 만들어서 할것
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