using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private AudioObject _prefab;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    public void PlaySound(AudioClip clip, float pitch = 1.0f, float volume = 1.0f)
    {
        AudioObject audio = Instantiate(_prefab, transform);
        audio.source.clip = clip;
        audio.source.pitch = pitch;
        audio.source.volume = volume;
        audio.source.Play();
    }
}
