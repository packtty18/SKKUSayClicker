
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
        Dictionary<EUpgradeType, Upgrade> _db =  UpgradeManager.Instance.GetUpgrade;
        for (int i = 0; i < _db.Count; i++)
        {
            UpgradeButton button = Instantiate(_upgradeButtonPrefab, _buttonRoot).GetComponent<UpgradeButton>();
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
