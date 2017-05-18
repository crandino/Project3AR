using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {

    public float max_life_time;
    private float life_time;

    private bool available;  // Player obtains balls if item has not been hit before

    TurretController turret_controller;
    AudioManager audio_manager;

	// Use this for initialization
	void InitItem()
    {
        audio_manager = GameObject.FindGameObjectWithTag("Audio_Manager").GetComponent<AudioManager>();
        turret_controller = GameObject.FindGameObjectWithTag("Cannon").GetComponent<TurretController>();

        life_time = 0.0f;
        available = true;
	}	
	
    public bool UpdateTime()
    {
        life_time += Time.deltaTime;
        return life_time > max_life_time;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Ball" && available)
        {
            audio_manager.PlayItemCrash();
            available = false;
            turret_controller.AddBalls(5);            
        }

        if (col.gameObject.tag == "Barrel" && available)
        {
            audio_manager.PlayItemCrash();
            available = false;
            max_life_time += 5.0f;
        }
    }
}
