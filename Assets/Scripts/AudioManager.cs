using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    AudioSource music_source;
    AudioSource effect_source;
    private bool music_on;

    public AudioClip level_music;
    public AudioClip[] metal_impacts;
    public AudioClip[] cannon_shots;

    private GameObject level_manager;

	// Use this for initialization
	void Start ()
    {
        music_source = gameObject.AddComponent<AudioSource>();
        effect_source = gameObject.AddComponent<AudioSource>();
        //music_on = false;
        //game_music = GameObject.Find("ARCamera").GetComponent<AudioSource>();
        //game_music.Stop();

        //level_manager = GameObject.FindGameObjectWithTag("GameController");
    }
	
	//// Update is called once per frame
	//void Update ()
 //   {
 //       if (level_manager.GetComponent<LevelManager>().GetCurrentPhase() == GAME_PHASES.GAME && !music_on)
 //       {
 //           effect_source.Play();
 //           music_on = true;
 //       }
        
 //       if (level_manager.GetComponent<LevelManager>().GetCurrentPhase() == GAME_PHASES.MENU && music_on)
 //       {
 //           game_music.Stop();
 //           music_on = false;
 //       }
	//}

    public void PlayCannon()
    {
        effect_source.PlayOneShot(cannon_shots[Random.Range(0, 2)]);
    }

    public void PlayMetalImpact()
    {
        effect_source.PlayOneShot(metal_impacts[Random.Range(0, 2)]);
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
