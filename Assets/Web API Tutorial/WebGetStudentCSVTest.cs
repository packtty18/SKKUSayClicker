using CsvHelper;
using CsvHelper.Configuration;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetStudentCSVTest : MonoBehaviour
{
    private const string URL =
        "https://raw.githubusercontent.com/mongilteacher/skku2_script_study/refs/heads/main/students.csv";

    private readonly List<Student> _students = new();

    private void Start()
    {
        LoadStudents().Forget();
    }

    private async UniTask LoadStudents()
    {
        string csvText = await GetWebText(URL);

        if (string.IsNullOrEmpty(csvText))
        {
            Debug.LogWarning("[WebGetStudentCSVTest] CSV is empty.");
            return;
        }

        csvText = csvText.TrimStart('\uFEFF'); // BOM 제거

        try
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MemberTypes = MemberTypes.Fields, // ⭐ 핵심
                MissingFieldFound = null,
                HeaderValidated = null
            };

            using var reader = new StringReader(csvText);
            using var csv = new CsvReader(reader, config);

            _students.Clear();
            _students.AddRange(csv.GetRecords<Student>());

            Debug.Log($"[CSV] Parsed {_students.Count} students");
            PrintList();
        }
        catch (Exception e)
        {
            Debug.LogError($"[CSV] Parse Error: {e}");
        }
    }

    private void PrintList()
    {
        foreach (Student student in _students)
        {
            Debug.Log($"ID : {student.ID}, Name : {student.Name}, Age : {student.Age}");
        }
    }

    private async UniTask<string> GetWebText(string url)
    {
        using UnityWebRequest request = UnityWebRequest.Get(url);

        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[WebGetStudentCSVTest] Error: {request.error}");
            return string.Empty;
        }

        return request.downloadHandler.text;
    }
}
