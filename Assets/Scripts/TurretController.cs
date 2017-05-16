using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{
    public GameObject ball_prefab;
    public float min_power_launch;
    public float max_power_launch;
    public float charger_time;

    private GameObject ui_manager;

    public int num_balls;

    private float start_time;
    [HideInInspector] public float final_time;

    [HideInInspector] public int game_defined_balls;

    private float power_units;
    private float power_tmp;

    private LevelManager level_manager;

    private AudioSource[] audio_sources;

	// Use this for initialization
	void Start ()
    {     
        level_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        power_units = max_power_launch / charger_time;
        game_defined_balls = num_balls;

        audio_sources = GameObject.FindGameObjectWithTag("Cannon").GetComponents<AudioSource>();

        ui_manager = GameObject.Find("UIManager");

        //Set start balls
        ui_manager.GetComponent<InGameUI>().SetCurrentBalls(num_balls);
    }

    // Update is called once per frame
    void Update()
    {
        if (level_manager.IsGameRunning())
        {
            // Track a single touch as a direction control.
            if (Input.touchCount > 0)
            {
                power_tmp = 0;
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    start_time = Time.time * 1000;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                    final_time = (Time.time * 1000) - start_time;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if(num_balls > 0)
                    {
                        //final_time = (Time.time * 1000) - start_time;
                        if (final_time > charger_time)
                        {
                            power_tmp = max_power_launch;
                        }
                        else
                        {
                            power_tmp = (power_units * final_time) + min_power_launch;
                        }

                        Vector3 ball_position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.75f, gameObject.transform.position.z);
                        GameObject ball_instance = (GameObject)Instantiate(ball_prefab, ball_position, Quaternion.identity);
                        ball_instance.GetComponent<Rigidbody>().AddForce(power_tmp * gameObject.transform.forward);

                        audio_sources[Random.Range(0, 2)].Play();
                        --num_balls; // One ball less

                        //Updating UI num balls
                        ui_manager.GetComponent<InGameUI>().SetCurrentBalls(num_balls);
                        final_time = 0;
                    }
                    else
                    {
                        level_manager.ChangeGameState(GAME_PHASES.LOSE);  // no more bullets!
                    }
                }
            }
        }
    }

    public void AddBalls(int extra_balls)
    {
        num_balls += extra_balls;
        ui_manager.GetComponent<InGameUI>().SetCurrentBalls(num_balls);
    }
}
