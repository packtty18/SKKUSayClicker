using DG.Tweening;
using UnityEngine;

public class CardpackObject : MonoBehaviour, IClickable
{
    [SerializeField] private ECardPackType _currentType;

    [Header("Spawn Motion")]
    [SerializeField] private float _moveDistance = 1.5f;
    [SerializeField] private float _moveDuration = 0.5f;


    public void Initialize()
    {
        PlaySpawnMotion();
    }

    private void PlaySpawnMotion()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector3 targetPos = transform.position + (Vector3)(randomDir * _moveDistance);

        transform.DOMove(targetPos, _moveDuration)
            .SetEase(Ease.OutCubic);

        Debug.Log("[CardPack] Spawn Motion Played");
    }

    public void OnClick(SClickInfo info)
    {
        AddToInventory();
    }

    private void AddToInventory()
    {
        //Inventory.Instance.AddPack();
        Debug.Log($"[CardPack] Added to Inventory : {_currentType}");

        Destroy(gameObject);
    }
}
