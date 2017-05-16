using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {

    public float max_life_time;
    private float life_time;

    private bool available;

    TurretController turret_controller;

	// Use this for initialization
	void Start ()
    {
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
            turret_controller.AddBalls(5);
        }

        if (col.gameObject.tag == "Barrel" && available)
        {
            available = false;
            max_life_time += 5.0f;
        }
    }
}
