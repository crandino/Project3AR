using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    AudioSource game_music;

	// Use this for initialization
	void Start ()
    {
        game_music = GameObject.Find("ARCamera").GetComponent<AudioSource>();
        game_music.Stop();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().GetCurrentPhase() == GAME_PHASES.GAME)
        {
            game_music.Play();
        }
        
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().GetCurrentPhase() == GAME_PHASES.MENU)
        {
            game_music.Stop();
        }
	
	}
}
