using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public enum GAME_PHASES
    {
        MENU,
        GAME
    }

    private GameObject ground_marker;
    private GameObject turret_marker;
    private GameObject cannon;

    // Types of enemies!
    public GameObject enemy_A;

    public bool debug_mode;

    float timer;
    float time_between_spawns;

    [HideInInspector] public List<GameObject> list_of_enemies;
    private List<GameObject> list_of_enemies_to_remove;

    private GAME_PHASES game_phase;

	// Use this for initialization
	void Start ()
    {
        turret_marker = GameObject.FindGameObjectWithTag("Pos_Turret");
        cannon = GameObject.FindGameObjectWithTag("Cannon");
        ground_marker = GameObject.FindGameObjectWithTag("Terrain");

        game_phase = GAME_PHASES.MENU;

        timer = 0.0f;
        time_between_spawns = 1.0f;

        list_of_enemies = new List<GameObject>();
        list_of_enemies_to_remove = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch(game_phase)
        {
            case (GAME_PHASES.MENU):
                {
                    if (AreTargetsReady() || debug_mode )
                        game_phase = GAME_PHASES.GAME;
                    break;
                }
               
            case (GAME_PHASES.GAME):
                {
                    UpdateEnemies();

                    if (timer > time_between_spawns)
                    {
                        GenerateEnemy();
                        timer = 0.0f;
                    }
                    else
                        timer += Time.deltaTime;
                }
                break;
        }   
	}

    void GenerateEnemy()
    {
        float random_distance = Random.Range(10.0f, 35.0f);
        float random_width = Random.Range(-4.5f, 4.5f);

        Vector3 enemy_position = (ground_marker.transform.position) - (ground_marker.transform.forward * random_distance);
        enemy_position.y = 0.75f;
        enemy_position.x += random_width;

        //Vector3 enemy_position = terrain.transform.position + new Vector3(random_pos.x, 0.75f, random_pos.y);
        GameObject enemy = Instantiate(enemy_A, enemy_position, Quaternion.identity) as GameObject;
        enemy.transform.parent = ground_marker.transform;
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

    private bool AreTargetsReady()
    {
        if (ground_marker.GetComponent<MarkerDetectionScript>().markerDetected() && turret_marker.GetComponent<MarkerDetectionScript>().markerDetected())
            return true;
        return false;
    }

    void OnTriggerEnter()
    {
        Debug.Log("YOU LOSE!");
    }
}
