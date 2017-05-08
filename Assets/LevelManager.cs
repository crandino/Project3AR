using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    private GameObject ground;
    private GameObject turret_marker;
    private GameObject cannon;

    public GameObject basic_enemy;

    float timer;
    float time_between_spawns;

    float max_distance_plane = 1.0f;

    bool ground_set = false;

	// Use this for initialization
	void Start ()
    {
        turret_marker = GameObject.FindGameObjectWithTag("Pos_Turret");
        cannon = GameObject.FindGameObjectWithTag("Cannon");

        // Ground generation
        ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.transform.localScale *= max_distance_plane;
        ground.transform.position = new Vector3(999.9f, 999.9f, 999.9f);

        timer = 0.0f;
        time_between_spawns = 3.0f;     
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if(!ground_set && cannon.GetComponent<Renderer>().enabled == true)
        //{
            ground.transform.position = turret_marker.transform.position;
        //    ground_set = true;
        //}           

        if (timer > time_between_spawns)
        {
            Vector2 random_pos = Random.insideUnitCircle * max_distance_plane;
            Vector3 enemy_position = turret_marker.transform.position + new Vector3(random_pos.x, 0.75f, random_pos.y);
            Instantiate(basic_enemy, enemy_position, Quaternion.identity);
            timer = 0.0f;
        }
        else
            timer += Time.deltaTime;     
	}
}
