using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

public class CardpackObject : MonoBehaviour, IClickable, IPlayFeedback, IFeedbackOwner
{
    [SerializeField] private ECardPackTheme _currentType;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private FeedbackPlayer _clickFeedback;

    [Header("Spawn Motion Settings")]
    [SerializeField] private float moveForce = 5f;     // Initial impulse force

    [Header("Caching")]
    public Transform OwnerTransform => transform;

    private DataManager _data => DataManager.Instance;

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
        float get = _data.GetData(EIncomeData.Theme1Price).Value * _data.GetData(EIncomeData.Theme1Price).Value;
        SFeedbackData data = new SFeedbackData()
        {
            TextType = EFloatTextType.Money,
            TextValue = get
        };

        _clickFeedback?.PlayFeedbacks(data);
        LeanPool.Despawn(gameObject);
        _data.GetData(ECurrentcyData.Money).Increase(get);
        //인벤토리에 돈 추가

    }


    public void Play()
    {
        PlaySpawnMotion();
    }
}
