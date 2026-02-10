using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
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
        string csv = await GetWebText(URL);
        Debug.Log(csv);

        ParseCSV(csv);
        PrintList();
    }

    private void ParseCSV(string csv)
    {
        _students.Clear();

        string[] lines = csv.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            string[] columns = line.Split(',');

            if (columns.Length < 3)
            {
                Debug.LogWarning($"[CSV] 올바르지 않는 양식 감지: {line}");
                continue;
            }

            if (!int.TryParse(columns[0], out int id))
            {
                Debug.LogWarning($"[CSV] id가 int형식이 아님: {line}");
                continue;
            }

            string name = columns[1];
            if (!int.TryParse(columns[2], out int age))
            {
                Debug.LogWarning($"[CSV] age가 int형식이 아님: {line}");
                continue;
            }

            _students.Add(new Student(id,name, age));
        }

        Debug.Log($"[CSV] Parsed {_students.Count} students");
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
