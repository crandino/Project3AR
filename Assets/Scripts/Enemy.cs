using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    // Public variables
    public uint points;
    public float min_speed;
    public float max_speed;
    public float max_time_before_disappear;

    private float speed;
    private float time_since_impact;
    private bool impacted;
    private bool ready_to_delete;

    private bool shield_active;

    private Color color_active;
    private Color color_inactive;

    private GameObject ground_marker;

    void Start()
    {
        color_inactive = Color.gray;
        color_active = Color.yellow;
        
        // Setting color
        GetComponent<Renderer>().material.color = color_active;

        // Type of enemies
        speed = Random.Range(min_speed, max_speed);
        shield_active = Random.Range(0, 2) == 1 ? true : false;

        if(shield_active)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        // Private variables to control impacts, 
        impacted = false;
        ready_to_delete = false;

        ground_marker = GameObject.FindGameObjectWithTag("Terrain");
    }

    public void UpdateMovement()
    {
        transform.Translate(ground_marker.transform.forward * speed * Time.deltaTime);  // Enemies follow plane 
       
        if (impacted)
        {
            if (time_since_impact > max_time_before_disappear)
                ready_to_delete = true;
            else
            {
                time_since_impact += Time.deltaTime;

                // Applying some alpha to the material
                Color col = GetComponent<Renderer>().material.color;
                col.a = Mathf.Lerp(1.0f, 0.0f, time_since_impact / max_time_before_disappear);
                GetComponent<Renderer>().material.color = col;
            }
        }
    }

    public void FixedUpdate()
    {
        if (impacted)
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Renderer>().material.color = color_inactive;
        }
        else if (!shield_active)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            transform.GetChild(0).gameObject.SetActive(false);            
        }
    }

    void OnCollisionEnter(Collision col)
    {
        // Colisions between ball and enemy
        if (col.gameObject.tag == "Ball")
        {
            if (shield_active)
            {
                shield_active = false;
            }
            else
            {
                impacted = true;
            }
            
            // Removing Rigidbody and Collider from the ball. Later, on ResetLevel() from
            // Level Manager, the Ball will be deleted.
            Destroy(col.gameObject.GetComponent<Rigidbody>());
            Destroy(col.gameObject.GetComponent<SphereCollider>());
            col.gameObject.transform.position = new Vector3(0.0f, -1000.0f, 0.0f);
        }

        //Colisions between enemies
        if (col.gameObject.tag == "Enemy")
        {
            if (!shield_active)
            {
                impacted = true;
            }
        }
    }

    public bool ReadyToDelete()
    {
        return ready_to_delete;
    }
}
