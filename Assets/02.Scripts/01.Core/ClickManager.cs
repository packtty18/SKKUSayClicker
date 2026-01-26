using UnityEngine;

/// <summary>
/// 플레이어의 마우스 클릭에 대한 클릭이 가능한 오브젝트의 실행
/// </summary>
public class ClickManager : MonoBehaviour
{
    [SerializeField] private LayerMask clickLayer;
    [SerializeField] private ClickDataProvider clickDataProvider;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        clickDataProvider = GetComponent<ClickDataProvider>();
        Debug.Log("[ClickManager] Initialized");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }
    }

    private void HandleClick(Vector2 screenPos)
    {
        Vector2 worldPos = _camera.ScreenToWorldPoint(screenPos);
        Collider2D hit = Physics2D.OverlapPoint(worldPos, clickLayer);

        if (hit == null)
        {
            Debug.Log("[ClickManager] Click Miss");
            return;
        }

        if (!hit.TryGetComponent<IClickable>(out var clickable))
        {
            Debug.Log("[ClickManager] Target is not clickable");
            return;
        }

        SClickInfo clickInfo = clickDataProvider.CreateManualClick(worldPos);
        clickable.OnClick(clickInfo);
    }
}
