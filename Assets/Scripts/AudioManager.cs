using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer masterMixer;

    public AudioSource sourcePrefab;

    void Start()
    {
        UpdateVolume();
    }


    public void PlayClip(AudioClip clip, AudioClipSettings settings)
    {
        if (clip && ApplicationManager.applicationData.playerData.soundsOn)
        {
            AudioSource source = Instantiate(sourcePrefab);
            source.pitch = Time.timeScale * (settings.pitch + Random.Range(-settings.pitchVariance / 2f, settings.pitchVariance / 2f));
            source.volume = settings.volume;
            source.clip = clip;
            source.Play();

            Destroy(source.gameObject, clip.length * 3);
        }
    }

    public void PlayClip(AudioClip clip)
    {
        PlayClip(clip, new AudioClipSettings());
    }

    public void PlayClip(string clipName)
    {
        PlayClip(clipName, new AudioClipSettings());
    }

    public void PlayClip(string clipName, AudioClipSettings settings)
    {
        if (AudioClipManager.Instance.GetAudioClip(clipName, out AudioClip clip))
        {
            PlayClip(clip, settings);
        }
    }

    public void UpdateVolume()
    {
        masterMixer.SetFloat("masterVolume", ApplicationManager.applicationData.playerData.soundsOn ? 0 : -80f);
    }
}

public class AudioClipSettings
{
    public float pitch = 1;
    public float pitchVariance = 0;
    public float volume = 1;
}