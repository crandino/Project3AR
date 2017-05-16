using UnityEngine;
using System.Collections.Generic;

public enum GAME_PHASES
{
    MENU = 0,
    GAME,
    WIN,
    LOSE
}

public class LevelManager : MonoBehaviour
{
    public bool debug_mode;
    public GameObject enemy;
    public GameObject item;

    public int score_to_reach;
    public int score;

    private float timer;
    public float time_between_spawns;
    public float time_between_items;
    private float last_time_spawn;
    private float last_time_items;

    private GameObject ground_marker;
    private GameObject turret_marker;
    private GameObject cannon;    

    [HideInInspector] public List<GameObject> list_of_enemies;
    [HideInInspector] public List<GameObject> list_of_items;
    private List<GameObject> list_of_enemies_to_remove;
    private List<GameObject> list_of_items_to_remove;

    private GAME_PHASES game_phase;

    public GAME_PHASES GetCurrentPhase()
    {
        return game_phase;
    }

	// Use this for initialization
	void Start ()
    {
        turret_marker = GameObject.FindGameObjectWithTag("Pos_Turret");
        cannon = GameObject.FindGameObjectWithTag("Cannon");
        ground_marker = GameObject.FindGameObjectWithTag("Terrain");

        game_phase = GAME_PHASES.MENU;

        timer = 0.0f;

        score = 0;

        list_of_enemies = new List<GameObject>();
        list_of_enemies_to_remove = new List<GameObject>();

        list_of_items = new List<GameObject>();
        list_of_items_to_remove = new List<GameObject>();

        //Set ui score
        GameObject.Find("UIManager").GetComponent<InGameUI>().SetCurrentScore(score);
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch(game_phase)
        {
            case (GAME_PHASES.MENU):
                {
                    if (AreTargetsReady() && GameObject.Find("UIManager").GetComponent<InGameUI>().game_start || debug_mode )
                    {
                        game_phase = GAME_PHASES.GAME;
                    }                        

                    break;
                }
               
            case (GAME_PHASES.GAME):
                {
                    if(score >= score_to_reach)
                    {
                        game_phase = GAME_PHASES.WIN;
                    }

                    //          ---- ENEMIES ----
                    // Each "time_between_spawns", a new enemy appears"       
                    if (timer - last_time_spawn > time_between_spawns)
                    {
                        GenerateEnemy();
                        last_time_spawn = timer;
                    }

                    // Update movement and states of enemies
                    UpdateEnemies();

                    //           ---- ITEMS ----
                    if (timer - last_time_items > time_between_items)
                    {
                        GenerateItem();
                        last_time_items = timer;
                    }

                    // Update items
                    UpdateItems();

                    timer += Time.deltaTime;  // Incrementing timer
                }
                break;

            case (GAME_PHASES.WIN):
                {
                    if(GameObject.Find("UIManager").GetComponent<InGameUI>().main_menu)
                    {
                        ResetLevel();
                        game_phase = GAME_PHASES.MENU;
                    }
                    break;
                }

            case (GAME_PHASES.LOSE):
                {
                    if (GameObject.Find("UIManager").GetComponent<InGameUI>().main_menu)
                    {
                        ResetLevel();
                        game_phase = GAME_PHASES.MENU;
                    }
                    break;
                }
        }   
	}

    void GenerateEnemy()
    {
        float random_distance = Random.Range(10.0f, 35.0f);
        float random_width = Random.Range(-4.5f, 4.5f);

        Vector3 enemy_position = (ground_marker.transform.position) + (ground_marker.transform.forward * random_distance);
        enemy_position += ground_marker.transform.up * 0.70f;
        //enemy_position += ground_marker.transform.right * random_width;

        //Vector3 enemy_position = terrain.transform.position + new Vector3(random_pos.x, 0.75f, random_pos.y);
        GameObject e = Instantiate(enemy, enemy_position, Quaternion.AngleAxis(180.0f, Vector3.up)) as GameObject;
        e.transform.parent = ground_marker.transform;
        list_of_enemies.Add(e);
    }

    void GenerateItem()
    {
        float random_distance = Random.Range(10.0f, 15.0f);
        float random_width = Random.Range(-4.5f, 4.5f);

        Vector3 item_position = (ground_marker.transform.position) + (ground_marker.transform.forward * random_distance);
        item_position += ground_marker.transform.up * 0.05f;
        //item_position += ground_marker.transform.right * random_width;

        //Vector3 enemy_position = terrain.transform.position + new Vector3(random_pos.x, 0.75f, random_pos.y);
        GameObject i = Instantiate(item, item_position, Quaternion.AngleAxis(-90.0f, Vector3.right)) as GameObject;
        i.transform.parent = ground_marker.transform;
        list_of_items.Add(i);
    }

    void UpdateEnemies()
    {
        bool score_changed = false;

        // Updating enemies
        foreach (GameObject e in list_of_enemies)
        {
            Enemy enemy_script = e.GetComponent<Enemy>();
            enemy_script.UpdateMovement();

            if (enemy_script.ReadyToDelete())
                list_of_enemies_to_remove.Add(e);
        }

        foreach (GameObject e in list_of_enemies_to_remove)
        {
            if (!score_changed)
            {
                score_changed = true;
            }

            score += 100;
            if(list_of_enemies.Remove(e))
                Destroy(e);
        }

        list_of_enemies_to_remove.Clear();  
        
        if(score_changed)
        {
            GameObject.Find("UIManager").GetComponent<InGameUI>().SetCurrentScore(score);
        }     
    }

    void UpdateItems()
    {
        // Updating enemies
        foreach (GameObject i in list_of_items)
        {
            Items item_script = i.GetComponent<Items>();

            if (item_script.UpdateTime())
                list_of_items_to_remove.Add(i);             
        }

        foreach (GameObject i in list_of_items_to_remove)
        {
            if (list_of_items.Remove(i))
                Destroy(i);
        }

        list_of_items_to_remove.Clear();
    }

    private bool AreTargetsReady()
    {
        if (ground_marker.GetComponent<MarkerDetectionScript>().markerDetected() && turret_marker.GetComponent<MarkerDetectionScript>().markerDetected())
            return true;
        return false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            game_phase = GAME_PHASES.LOSE;
            Debug.Log("YOU LOSE!");
        }            
    }

    void ResetLevel()
    {
        // Deleting all enemies
        foreach (GameObject e in list_of_enemies)
        {
            Destroy(e);                
        }
        list_of_enemies.Clear();
        list_of_enemies_to_remove.Clear();

        // Deleting all items
        foreach (GameObject i in list_of_items)
        {
            Destroy(i);
        }
        list_of_items.Clear();
        list_of_items_to_remove.Clear();

        // Deleting all balls
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject b in balls)
        {
            Destroy(b);
        }

        //Reset cannon bullets
        cannon.GetComponent<TurretController>().num_balls = cannon.GetComponent<TurretController>().game_defined_balls;
        GameObject.Find("UIManager").GetComponent<InGameUI>().SetCurrentBalls(cannon.GetComponent<TurretController>().game_defined_balls);

        // Resetting timers
        last_time_spawn = 0.0f;
        last_time_items = 0.0f;
        timer = 0.0f;
    }

    public void ChangeGameState(GAME_PHASES phase)
    {
        game_phase = phase;
    }

    public bool IsGameRunning()
    {
        return game_phase == GAME_PHASES.GAME;
    }
}
