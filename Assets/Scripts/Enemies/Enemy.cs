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
    private bool ready_to_delete;

    public bool impacted;
    public bool shield_active;

    private Color color_active;
    private Color color_inactive;

    private GameObject ground_marker;
    private GameObject barrel;
    private GameObject shield;

    private AudioSource[] metal_audio_sources;
    private AudioSource[] wood_audio_sources;

    void Start()
    {
        color_inactive = Color.gray;
        color_active = Color.yellow;

        ground_marker = GameObject.FindGameObjectWithTag("Terrain");

        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform tmp = transform.GetChild(i);

            if (tmp.name == "Barrel")
                barrel = tmp.gameObject;
            else if(tmp.name == "Shield")
                shield = tmp.gameObject;
            else if(tmp.name == "MetalImpacts")
                metal_audio_sources = tmp.gameObject.GetComponents<AudioSource>();
        }
        
        // Setting color
        //GetComponent<Renderer>().material.color = color_active;

        // Type of enemies
        speed = Random.Range(min_speed, max_speed);
        shield_active = Random.Range(0, 2) == 1 ? true : false;

        if(shield_active)
        {
            barrel.GetComponent<Rigidbody>().isKinematic = true;
            shield.SetActive(true);
        }

        //wood_audio_sources = GetComponents<AudioSource>();
        //metal_audio_sources = transform.GetChild(1).GetComponents<AudioSource>();

        // Private variables to control impacts, 
        impacted = false;
        ready_to_delete = false;

        
    }

    public void UpdateMovement()
    {
        Vector3 dir = new Vector3(ground_marker.transform.forward.x, 0.0f, ground_marker.transform.forward.z);
        dir.Normalize();
        transform.Translate(dir * speed * Time.deltaTime);  // Enemies follow plane
       
        if (impacted)
        {
            if (time_since_impact > max_time_before_disappear)
                ready_to_delete = true;
            else
            {
                time_since_impact += Time.deltaTime;

                // Applying some alpha to the material
                //Color col = GetComponent<Renderer>().material.color;
                //col.a = Mathf.Lerp(1.0f, 0.0f, time_since_impact / max_time_before_disappear);
                //GetComponent<Renderer>().material.color = col;
            }
        }
    }

    public void FixedUpdate()
    {
        if (impacted)
        {
            barrel.GetComponent<Rigidbody>().useGravity = true;
            //GetComponent<Renderer>().material.color = color_inactive;
        }
        else if (!shield_active)
        {
            barrel.GetComponent<Rigidbody>().isKinematic = false;
            shield.SetActive(false);            
        }
    }

    public bool ReadyToDelete()
    {
        return ready_to_delete;
    }

    public void PlayMetalImpactSound()
    {
        metal_audio_sources[Random.Range(0, 2)].Play();
    }
}
