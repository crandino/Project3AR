using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {

    public float max_life_time;
    private float life_time;

    TurretController turret_controller;

	// Use this for initialization
	void Start ()
    {
        turret_controller = GameObject.FindGameObjectWithTag("Pos_Turret").GetComponent<TurretController>();
        life_time = 0.0f;
	}	
	
    public bool UpdateTime()
    {
        life_time += Time.deltaTime;
        return life_time > max_life_time;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Ball")
        {
            turret_controller.AddBalls(5);
        }
    }
}
