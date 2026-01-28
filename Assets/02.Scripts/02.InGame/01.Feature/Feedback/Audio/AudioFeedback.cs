using Unity.Cinemachine;
using UnityEngine;

public class AudioFeedback : FeedbackBase
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _clip;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }
    public override void Play(SFeedbackData data)
    {
        if(_audio.isPlaying)
        {
            _audio.Stop();
        }
        
        _audio.pitch = Random.Range(0.8f, 1.2f);
        _audio.clip = _clip;
        _audio.Play();
    }
}
