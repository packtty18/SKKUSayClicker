using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "SO/UI/Upgrade")]
public class UpgradeDBSO : ScriptableObject
{
    [SerializeField]
    public List<SUpgradeUIInfo> Datas = new();

    private void OnEnable()
    {
        ValidateDatas();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ValidateDatas();
    }
#endif

    [Button("검증하기")]
    private void ValidateDatas()
    {
        if (Datas == null || Datas.Count == 0)
        {
            Debug.LogWarning("[UpgradeUISO] Datas list is empty.", this);
            return;
        }

        HashSet<string> nameSet = new HashSet<string>();

        for (int i = 0; i < Datas.Count; i++)
        {
            var data = Datas[i];


            //if (data.Id <= 0)
            //{
            //    Debug.LogError($"[UpgradeUISO] Invalid ID at index {i}", this);
            //}

            if (string.IsNullOrWhiteSpace(data.Name))
            {
                Debug.LogError($"[UpgradeUISO] Empty Name at index {i}", this);
            }

            if (data.Icon == null)
            {
                Debug.LogWarning($"[UpgradeUISO] Icon is missing. Name: {data.Name}", this);
            }

            if (string.IsNullOrWhiteSpace(data.Description))
            {
                Debug.LogWarning($"[UpgradeUISO] Description is empty. Name: {data.Name}", this);
            }

            if (!string.IsNullOrWhiteSpace(data.Name))
            {
                if (!nameSet.Add(data.Name))
                {
                    Debug.LogError(
                        $"[UpgradeUISO] Duplicate Name detected: \"{data.Name}\"", this);
                }
            }
        }

        Debug.Log($"[UpgradeUISO] Validation finished. Total: {Datas.Count}", this);
    }
}