using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

public class Product : MonoBehaviour, IClickable, IPlayFeedback, IFeedbackOwner
{
    [SerializeField] private EProductType _currentType;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private FeedbackPlayer _clickFeedback;

    [Header("Spawn Motion Settings")]
    [SerializeField] private float moveForce = 5f;     // Initial impulse force

    [Header("Caching")]
    public Transform OwnerTransform => transform;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
    }

    public void Initialize()
    {
        PlaySpawnMotion();
    }



    [Button]
    public void PlaySpawnMotion()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;

        _rigidbody.linearVelocity = Vector2.zero; 
        _rigidbody.AddForce(randomDir * moveForce, ForceMode2D.Impulse);

        Debug.Log("[CardPack] Spawn Rigidbody Motion Played");
    }

    public void OnClick()
    {
        float get = 100; // 나중에 데이터화하여 참조
        FeedbackData data = new FeedbackData()
        {
            TextType = EFloatTextType.Money,
            TextValue = get
        };

        _clickFeedback?.PlayFeedbacks(data);

        CurrencyManager.Instance.Add(ECurrencyType.Money, get);

        Utils.ObjectDestroy(gameObject); 
    }


    public void Play()
    {
        PlaySpawnMotion();
    }
}
