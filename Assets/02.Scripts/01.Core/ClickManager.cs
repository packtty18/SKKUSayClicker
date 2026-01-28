using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어의 마우스 클릭에 대한 클릭 가능한 오브젝트의 실행 및 커서/이펙트 관리
/// </summary>
public class ClickManager : MonoBehaviour
{
    [Header("Click Detection")]
    [SerializeField] private LayerMask clickLayer;

    [Header("Cursor Settings")]
    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D hoverCursor;
    [SerializeField] private Texture2D clickCursor;
    [SerializeField] private Texture2D clickableClickCursor;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;
    [SerializeField] private float clickCursorDuration = 0.1f;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem normalClickParticle;
    [SerializeField] private ParticleSystem clickableObjectParticle;

    private Camera _camera;
    private bool _isHoveringClickable;
    private bool _isClickAnimating;

    private void Awake()
    {
        _camera = Camera.main;
        SetCursor(normalCursor);
        Debug.Log("[ClickManager] Initialized");
    }

    private void Update()
    {
        UpdateCursor();

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }
    }

    private void UpdateCursor()
    {
        // 클릭 애니메이션 중에는 커서 업데이트 스킵
        if (_isClickAnimating)
        {
            return;
        }

        Vector2 worldPos = ConvertToWorldPosition(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos, clickLayer);

        bool isCurrentlyHovering = hit != null && hit.TryGetComponent<IClickable>(out _);

        if (isCurrentlyHovering != _isHoveringClickable)
        {
            _isHoveringClickable = isCurrentlyHovering;
            SetCursor(_isHoveringClickable ? hoverCursor : normalCursor);
        }
    }

    private void HandleClick(Vector2 screenPos)
    {
        Vector2 worldPos = ConvertToWorldPosition(screenPos);
        Collider2D hit = Physics2D.OverlapPoint(worldPos, clickLayer);

        if (hit == null)
        {
            Debug.Log("[ClickManager] Click Miss");
            PlayParticleEffect(normalClickParticle, worldPos);
            StartCoroutine(PlayClickCursorAnimation(clickCursor, normalCursor));
            return;
        }

        if (!TryExecuteClick(hit))
        {
            Debug.Log($"[ClickManager] Target is not clickable: {hit.name}");
            PlayParticleEffect(normalClickParticle, worldPos);
            StartCoroutine(PlayClickCursorAnimation(clickCursor, normalCursor));
        }
        else
        {
            PlayParticleEffect(clickableObjectParticle, worldPos);
            StartCoroutine(PlayClickCursorAnimation(clickableClickCursor, hoverCursor));
        }
    }

    private IEnumerator PlayClickCursorAnimation(Texture2D clickTexture, Texture2D returnTexture)
    {
        _isClickAnimating = true;

        // 클릭 커서로 변경
        SetCursor(clickTexture);

        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(clickCursorDuration);

        // 원래 커서로 복귀
        SetCursor(returnTexture);

        _isClickAnimating = false;
    }

    private Vector2 ConvertToWorldPosition(Vector2 screenPos)
    {
        return _camera.ScreenToWorldPoint(screenPos);
    }

    private bool TryExecuteClick(Collider2D target)
    {
        if (!target.TryGetComponent<IClickable>(out var clickable))
        {
            return false;
        }

        clickable.OnClick();
        return true;
    }

    private void SetCursor(Texture2D cursorTexture)
    {
        if (cursorTexture == null)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
        }
    }

    private void PlayParticleEffect(ParticleSystem particle, Vector2 position)
    {
        if (particle == null)
        {
            return;
        }

        particle.transform.position = position;
        particle.Play();
    }

    private void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}