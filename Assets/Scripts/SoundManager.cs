using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musiceSource;
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get { return _instance; }
    }

    public float lowPitchRange = .95f;
    public float highPitcheRange = 1.05f;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void PlayRandom(params AudioClip[] clips)
    {
        efxSource.pitch = Random.Range(lowPitchRange, highPitcheRange);
        efxSource.clip = clips.RandomElement();
        efxSource.Play();
    }
}
