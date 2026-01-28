using Lean.Pool;
using Sirenix.OdinInspector;

using UnityEngine;

/// <summary>
/// Spawns feedback prefab and plays it if possible.
/// </summary>
public class SpawnFeedback : FeedbackBase
{
    [Header("Spawn")]
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private Vector2 _offset;

    [Header("Noise")]
    [SerializeField] private bool _noise = false; // 0 = no noise
    [SerializeField, ShowIf(nameof(_noise))] private float _noiseX = 0;
    [SerializeField, ShowIf(nameof(_noise))] private float _noiseY =0 ;
    public override void Play(SFeedbackData data)
    {
        Debug.Log("[SpawnFeedback] Play");

        GameObject spawned = LeanPool.Spawn(_targetPrefab,OwnerTransform.position, Quaternion.identity );

        if(spawned == null )
        {
            spawned = Instantiate(_targetPrefab);
            spawned.transform.position = OwnerTransform.position;
            spawned.transform.rotation = Quaternion.identity;
        }

        spawned.transform.localPosition += (Vector3)GetFinalOffset();
        CheckInterface(spawned, data);
    }

    private Vector2 GetFinalOffset()
    {
        if (!_noise)
            return _offset;

        float noiseX = Random.Range(-_noiseX, _noiseX);
        float noiseY = Random.Range(-_noiseY, _noiseY);

        return _offset + new Vector2(noiseX, noiseY);
    }

    protected virtual void CheckInterface(GameObject target , SFeedbackData data)
    {
        if (target.TryGetComponent<IPlayFeedback>(out var playable))
        {
            playable.Play();
        }
    }
}
