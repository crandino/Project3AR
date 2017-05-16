using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    AudioSource game_music;
    private bool music_on;

    private GameObject level_manager;

	// Use this for initialization
	void Start ()
    {
        music_on = false;
        game_music = GameObject.Find("ARCamera").GetComponent<AudioSource>();
        game_music.Stop();

        level_manager = GameObject.FindGameObjectWithTag("GameController");

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (level_manager.GetComponent<LevelManager>().GetCurrentPhase() == GAME_PHASES.GAME && !music_on)
        {
            game_music.Play();
            music_on = true;
        }
        
        if (level_manager.GetComponent<LevelManager>().GetCurrentPhase() == GAME_PHASES.MENU && music_on)
        {
            game_music.Stop();
            music_on = false;
        }
	
	}
}
