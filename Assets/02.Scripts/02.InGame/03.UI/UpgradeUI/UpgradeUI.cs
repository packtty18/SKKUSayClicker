
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : SerializedMonoBehaviour
{
    [Title("Required")]
    [SerializeField, Required] private GameObject _upgradeButtonPrefab;

    [SerializeField, Required] private Transform _buttonRoot;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        CheckRootChild();
        InstantButtons();
    }

    private void InstantButtons()
    {
        IReadOnlyDictionary< EUpgradeType, Upgrade> db = UpgradeManager.Instance.Upgrades;

        foreach (var pair in db)
        {
            EUpgradeType type = pair.Key;
            Upgrade upgrade = pair.Value;

            UpgradeButtonUI button =
                Instantiate(_upgradeButtonPrefab, _buttonRoot)
                .GetComponent<UpgradeButtonUI>();

            button.SetContent(type, upgrade);
        }
    }

    private void CheckRootChild()
    {
        if (_buttonRoot.childCount != 0)
        {
            for (int i = _buttonRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(_buttonRoot.GetChild(i).gameObject);
            }
        }
    }
}
