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
    private int score;

    private float timer;
    public float max_time_spawns;
    public float min_time_spawns;
    public float max_time_items;
    public float min_time_items;
    private float last_time_spawn;
    private float last_time_items;

    private float time_between_spawns;
    private float time_between_items;

    private GameObject ground_marker;
    private GameObject turret_marker;
    private GameObject cannon;

    private InGameUI ui_manager;

    [HideInInspector] public List<GameObject> list_of_enemies;
    [HideInInspector] public List<GameObject> list_of_items;
    private List<GameObject> list_of_enemies_to_remove;
    private List<GameObject> list_of_items_to_remove;

    private GAME_PHASES game_phase;

    private AudioManager audio_manager;

	// Use this for initialization
	void Start ()
    {
        audio_manager = GameObject.FindGameObjectWithTag("Audio_Manager").GetComponent<AudioManager>();
        turret_marker = GameObject.FindGameObjectWithTag("Pos_Turret");
        cannon = GameObject.FindGameObjectWithTag("Cannon");
        ground_marker = GameObject.FindGameObjectWithTag("Terrain");

        game_phase = GAME_PHASES.MENU;

        timer = 0.0f;
        time_between_items = Random.Range(min_time_items, max_time_items);
        time_between_spawns = Random.Range(min_time_spawns, max_time_spawns);

        score = 0;

        list_of_enemies = new List<GameObject>();
        list_of_enemies_to_remove = new List<GameObject>();

        list_of_items = new List<GameObject>();
        list_of_items_to_remove = new List<GameObject>();

        ui_manager = GameObject.Find("UIManager").GetComponent<InGameUI>();

        //Set ui score
        ui_manager.SetCurrentScore(score);
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch(game_phase)
        {
            case (GAME_PHASES.MENU):
                {
                    if (AreTargetsReady() && ui_manager.game_start || debug_mode )
                    {
                        audio_manager.PlayMusic();
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
                        time_between_spawns = Random.Range(min_time_spawns, max_time_spawns);
                    }

                    // Update movement and states of enemies
                    UpdateEnemies();

                    //           ---- ITEMS ----
                    if (timer - last_time_items > time_between_items)
                    {
                        GenerateItem();
                        last_time_items = timer;
                        time_between_items = Random.Range(min_time_items, max_time_items);
                    }

                    // Update items
                    UpdateItems();

                    timer += Time.deltaTime;  // Incrementing timer
                }
                break;

            case (GAME_PHASES.WIN):
                {
                    if(ui_manager.main_menu)
                    {
                        ResetLevel();
                        game_phase = GAME_PHASES.MENU;
                    }
                    break;
                }

            case (GAME_PHASES.LOSE):
                {
                    if (ui_manager.main_menu)
                    {
                        ResetLevel();
                        game_phase = GAME_PHASES.MENU;
                    }
                    break;
                }
        }   
	}

    void FixedUpdate()
    {
        switch (game_phase)
        {
            case (GAME_PHASES.GAME):
                {
                    // Update movement and states of enemies
                    UpdateEnemies();

                    // Update items
                    UpdateItems();
                }
                break;
        }
    }

    void GenerateEnemy()
    {
        float random_distance = Random.Range(15.0f, 30.0f);
        float random_width = Random.Range(-4.5f, 4.5f);

        Vector3 enemy_position = (ground_marker.transform.position) + (ground_marker.transform.forward * random_distance);
        enemy_position += ground_marker.transform.up * 0.65f;
        enemy_position += ground_marker.transform.right * random_width;

        GameObject e = Instantiate(enemy, enemy_position, Quaternion.AngleAxis(180.0f, ground_marker.transform.up)) as GameObject;
        e.transform.parent = ground_marker.transform;
        e.GetComponent<Enemy>().InitEnemy();        
        list_of_enemies.Add(e);
    }

    void GenerateItem()
    {
        float random_distance = Random.Range(10.0f, 15.0f);
        float random_width = Random.Range(-4.5f, 4.5f);

        Vector3 item_position = (ground_marker.transform.position) + (ground_marker.transform.forward * random_distance);
        item_position += ground_marker.transform.up * 0.05f;
        item_position += ground_marker.transform.right * random_width;

        GameObject i = Instantiate(item, item_position, Quaternion.AngleAxis(-90.0f, Vector3.right)) as GameObject;
        i.transform.parent = ground_marker.transform;
        i.GetComponent<Items>().InitItem();
        list_of_items.Add(i);
    }

    void UpdateEnemies()
    {
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
            if(list_of_enemies.Remove(e))
                Destroy(e);
        }

        list_of_enemies_to_remove.Clear();  
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
        if(col.gameObject.tag == "Barrel")
        {
            game_phase = GAME_PHASES.LOSE;
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
        ui_manager.SetCurrentBalls(cannon.GetComponent<TurretController>().game_defined_balls);

        // Resetting timers
        last_time_spawn = 0.0f;
        last_time_items = 0.0f;
        timer = 0.0f;

        // Reset Score value and UI
        score = 0;   
        ui_manager.SetCurrentScore(score);  

        // Stop music
        audio_manager.StopMusic();
    }

    public void ChangeGameState(GAME_PHASES phase)
    {
        game_phase = phase;
    }

    public bool IsGameRunning()
    {
        return game_phase == GAME_PHASES.GAME;
    }

    public GAME_PHASES GetCurrentPhase()
    {
        return game_phase;
    }

    public void UpdateScore(int extra_score = 0)
    {
        score += extra_score;
        ui_manager.SetCurrentScore(score);
    }  
}
