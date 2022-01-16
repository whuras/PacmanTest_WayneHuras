using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private AudioSource[] audioSources;

    private void Awake() => MaintainSingleton();

    void Start() => audioSources = GetComponents<AudioSource>();

    public void PlayIntro() => audioSources[0].Play();

    public void PlayEatGhost() => audioSources[1].Play();

    public void PlayDeath() => audioSources[3].Play();

    public void PlayMunch()
    {
        if(!audioSources[2].isPlaying)
            audioSources[2].Play();
    }

    private void MaintainSingleton()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
