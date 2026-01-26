using UnityEngine;

public class UIFloaterFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private GameObject _floatingUIPrefab;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _offset;

    public void Play(SClickInfo info)
    {
        Debug.Log("[UIFloaterFeedback] Play");

        FloatingText floater = Instantiate(_floatingUIPrefab, _target)
            .GetComponent<FloatingText>();

        floater.transform.localPosition = _offset;

        floater.Play(info.Power);
    }
}
