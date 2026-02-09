using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSpecTableSO", menuName = "SO/UpgradeSpecTableSO")]
public class UpgradeSpecTableSO : ScriptableObject
{
    [SerializeField] public List<UpgradeSpecData> Datas;

    [Button]
    public void CheckValidate()
    {
        List<string> errors = new List<string>();

        // 1. 데이터 존재 여부 검증
        if (Datas == null || Datas.Count == 0)
        {
            throw new InvalidOperationException("[UpgradeSpecTableSO] 데이터가 0개");
        }

        // 2. UpgradeType이 중복되었다면 오류
        var duplicateTypes = Datas
            .GroupBy(d => d.Type)
            .Where(g => g.Count() > 1)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToList();

        if (duplicateTypes.Count > 0)
        {
            foreach (var duplicate in duplicateTypes)
            {
                errors.Add($"중복된 Type 발견: {duplicate.Type})");
            }
        }

        // 3. 개별 SpecData 유효성 검증
        for (int i = 0; i < Datas.Count; i++)
        {
            var data = Datas[i];

            // Null 체크
            if (data == null)
            {
                errors.Add($"Index [{i}]: Null 데이터가 존재");
                continue;
            }

            // IsValid 검증
            if (!data.IsValid(out string errorMessage))
            {
                errors.Add($"Index [{i}]: {errorMessage}");
            }
        }

        //에러 코드 완성
        if (errors.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[UpgradeSpecTableSO] 검증 실패:");
            sb.AppendLine($"총 {errors.Count}개의 잘못된 값이 발견되었습니다.");
            sb.AppendLine();

            for (int i = 0; i < errors.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {errors[i]}");
            }

            sb.AppendLine();
            sb.AppendLine("게임을 실행할 수 없습니다. UpgradeSpecTableSO를 수정한 후 다시 시도하세요.");

            throw new InvalidOperationException(sb.ToString());
        }

        Debug.Log($"[UpgradeSpecTableSO] 검증 성공: {Datas.Count}개의 UpgradeSpecData가 유효합니다.");
    }
}
