using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private GameObject ground;
    private GameObject turret_marker;
    private GameObject cannon;

    // Types of enemies!
    public GameObject enemy_A;
    public GameObject enemy_B;
    public GameObject enemy_C;
    public GameObject enemy_D;

    float timer;
    float time_between_spawns;

    public List<GameObject> list_of_enemies;
    private List<GameObject> list_of_enemies_to_remove;

	// Use this for initialization
	void Start ()
    {
        turret_marker = GameObject.FindGameObjectWithTag("Pos_Turret");
        cannon = GameObject.FindGameObjectWithTag("Cannon");
        ground = GameObject.FindGameObjectWithTag("Terrain");

        // Ground generation
        //ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ////ground.GetComponent<MeshRenderer>().enabled = false;
        //ground.transform.localScale *= max_distance_plane;
        //ground.transform.position = new Vector3(999.9f, 999.9f, 999.9f);

        timer = 0.0f;
        time_between_spawns = 1.0f;

        list_of_enemies = new List<GameObject>();
        list_of_enemies_to_remove = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (!ground_set && cannon.GetComponent<Renderer>().enabled == true)
        //{
        //    ground.transform.position = turret_marker.transform.position;
        //    ground_set = true;
        //}

        UpdateEnemies();

        if (timer > time_between_spawns)
        {
            GenerateEnemy();
            timer = 0.0f;
        }
        else
            timer += Time.deltaTime;     
	}

    void GenerateEnemy()
    {
        float random_distance = Random.Range(8.0f, 30.0f);
        float random_width = Random.Range(-4.0f, 4.0f);

        Vector3 enemy_position = (ground.transform.position) - (ground.transform.forward * random_distance);
        enemy_position.y = 0.75f;
        enemy_position.x += random_width;

        //Vector3 enemy_position = terrain.transform.position + new Vector3(random_pos.x, 0.75f, random_pos.y);
        GameObject enemy = Instantiate(enemy_A, enemy_position, Quaternion.identity) as GameObject;
        enemy.transform.parent = ground.transform;
        list_of_enemies.Add(enemy);
    }

    void UpdateEnemies()
    {
        // Updating enemies
        foreach (GameObject e in list_of_enemies)
        {
            Enemy enemy_script = e.GetComponent<Enemy>();

            if (enemy_script.ReadyToDelete())
                list_of_enemies_to_remove.Add(e);
            else
            {
                enemy_script.UpdateEnemy();
            }
        }
        
        foreach (GameObject e in list_of_enemies_to_remove)
        {
            if(list_of_enemies.Remove(e)) Destroy(e);
        }       
    }

    void OnTriggerEnter()
    {
        Debug.Log("YOU LOSE!");
    }
}
