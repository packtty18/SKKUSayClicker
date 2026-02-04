using JetBrains.Annotations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour, IFeedbackOwner
{
    [SerializeField, ReadOnly] private Button _button;

    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;

    private EUpgradeType _type;
    private IReadOnlyUpgrade _upgrade;

    public Transform OwnerTransform => transform;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        UpgradeManager.Instance.OnDataChanged.Subscribe(Refresh);
    }

    private void OnDestroy()
    {
        if(UpgradeManager.Instance == null)
        {
            return;
        }
        UpgradeManager.Instance.OnDataChanged.Unsubscribe(Refresh);
    }

    public void SetContent(IReadOnlyUpgrade upgrade)
    {
        _upgrade = upgrade;
        _type = upgrade.Spec.Type;
        _iconImage.sprite = upgrade.Spec.Icon;
        _nameText.text = upgrade.Spec.Name;
        _descriptionText.text = upgrade.Spec.Description;
        _levelText.text = upgrade.Level.ToString();

        Refresh();
    }

    private void Refresh()
    {
        if (_upgrade == null)
            return;

        _levelText.text = $"{_upgrade.Level}";

        if(_upgrade.IsMaxLevel)
        {
            _costText.text = "MAX";
        }
        else
        {
            string icon = "";
            switch (_upgrade.Spec.CostType)
            {
                case ECurrencyType.Money:
                    icon = "<sprite=1>";
                    break;
                case ECurrencyType.Prestigy:
                    icon = "<sprite=2>";
                    break;
                default:
                    break;
            }

            _costText.text = icon + _upgrade.Cost.ToString();
        }
       
        _button.interactable = !_upgrade.IsMaxLevel;
    }

    public void OnClick()
    {
        //아직 피드백 완성 안됨
        if (UpgradeManager.Instance.TryLevelUp(_type))
        {
            Refresh();
        }
        else
        {

        }
    }
}
