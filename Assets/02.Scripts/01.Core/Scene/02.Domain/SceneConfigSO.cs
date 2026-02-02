using UnityEngine;

[CreateAssetMenu(fileName = "SceneConfigSO", menuName = "SO/Scene/SceneConfig")]
public class SceneConfigSO : ScriptableObject
{
    public ESceneType sceneType;
    public string sceneName;
}
