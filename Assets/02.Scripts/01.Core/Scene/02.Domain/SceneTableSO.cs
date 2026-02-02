using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector;
#endif

[CreateAssetMenu(fileName = "SceneTableSO", menuName = "SO/Scene/SceneTable")]
public class SceneTableSO : ScriptableObject
{
    [SerializeField] private List<SceneConfigSO> scenes;

    public string GetSceneName(ESceneType type)
    {
        foreach (var scene in scenes)
        {
            if (scene.sceneType == type)
                return scene.sceneName;
        }

        Debug.LogError($"[SceneTableSO] Scene not found: {type}");
        return string.Empty;
    }

#if UNITY_EDITOR
    [Button("Validate Scene Table")]
    private void Validate()
    {
        Debug.Log("[SceneTableSO] Validation started");

        if (scenes == null || scenes.Count == 0)
        {
            Debug.LogError("[SceneTableSO] Scene list is empty");
            return;
        }

        HashSet<ESceneType> duplicatedCheck = new();

        foreach (var config in scenes)
        {
            if (config == null)
            {
                Debug.LogError("[SceneTableSO] SceneConfigSO is null");
                continue;
            }

            // SceneType 중복 검사
            if (!duplicatedCheck.Add(config.sceneType))
            {
                Debug.LogError($"[SceneTableSO] Duplicated SceneType: {config.sceneType}");
            }

            // SceneName 비어 있음
            if (string.IsNullOrEmpty(config.sceneName))
            {
                Debug.LogError($"[SceneTableSO] SceneName is empty ({config.sceneType})");
                continue;
            }

            // 실제 씬 존재 여부
            if (!SceneExistsInBuildSettings(config.sceneName))
            {
                Debug.LogError($"[SceneTableSO] Scene not found in BuildSettings: {config.sceneName}");
            }
        }

        Debug.Log("[SceneTableSO] Validation finished");
    }

    private bool SceneExistsInBuildSettings(string sceneName)
    {
        foreach (var scene in EditorBuildSettings.scenes)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
            if (fileName == sceneName)
                return true;
        }
        return false;
    }
#endif
}
