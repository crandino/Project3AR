using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    AudioSource music_source;
    AudioSource effect_source;

    public AudioClip level_music;
    public AudioClip[] metal_impacts;
    public AudioClip[] cannon_shots;
    public AudioClip[] wood_crashes;
    public AudioClip item_crash;

    // Use this for initialization
    void Start ()
    {
        music_source = gameObject.AddComponent<AudioSource>();
        music_source.playOnAwake = false;
        effect_source = gameObject.AddComponent<AudioSource>();
        effect_source.playOnAwake = false;
    }

    public void PlayCannon()
    {
        effect_source.PlayOneShot(cannon_shots[Random.Range(0, 2)]);
    }

    public void PlayMetalImpact()
    {
        effect_source.PlayOneShot(metal_impacts[Random.Range(0, 2)]);
    }

    public void PlayWoodCrash()
    {
        effect_source.PlayOneShot(wood_crashes[Random.Range(0, 2)]);
    }

    public void PlayItemCrash()
    {
        effect_source.PlayOneShot(item_crash);
    }

    public void PlayMusic()
    {
        music_source.PlayOneShot(level_music);
    }

    public void StopMusic()
    {
        music_source.Stop();
    }
}
