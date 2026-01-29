using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour, IFeedbackOwner
{
    [SerializeField, ReadOnly] private Button _button;

    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;

    private EUpgradeType _type;
    private Upgrade _upgrade;

    public Transform OwnerTransform => transform;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnEnable()
    {
        UpgradeManager.OnDataChanged += Refresh;
    }

    private void OnDisable()
    {
        UpgradeManager.OnDataChanged -= Refresh;
    }

    public void SetContent(EUpgradeType type, Upgrade upgrade)
    {
        _type = type;
        _upgrade = upgrade;

        _iconImage.sprite = upgrade.SpecData.Icon;
        _nameText.text = upgrade.SpecData.Name;
        _descriptionText.text = upgrade.SpecData.Description;

        Refresh();
    }

    private void Refresh()
    {
        if (_upgrade == null)
            return;

        _levelText.text = $"Lv {_upgrade.Level}";
        _costText.text = _upgrade.IsMaxLevel ? "MAX" : _upgrade.Cost.ToString();

        _button.interactable = !_upgrade.IsMaxLevel;
    }

    public void OnClick()
    {
        //아직
    }
}
