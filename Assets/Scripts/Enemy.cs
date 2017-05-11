using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public uint points;
    public float speed;
    public float max_time_before_disappear;

    private float time_since_impact;
    private bool impacted;
    private bool ready_to_delete;

    private LevelManager level_manager;

    void Start()
    {
        level_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();

        impacted = false;
        ready_to_delete = false;
    }

	public void UpdateEnemy()
    {
        //Vector3 dir = transform.forward;
        //dir.x += Mathf.Sin(transform.position.z);
        //dir.Normalize();
        //dir *= (speed * Time.deltaTime);
        //transform.Translate(dir);

        transform.Translate(transform.forward * speed * Time.deltaTime);  // Enemies follow plane 

        if(impacted)
        {
            if(time_since_impact > max_time_before_disappear)
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

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            GetComponent<Rigidbody>().useGravity = true;
            impacted = true;

            // Ball disappears when hit a cylinder
            //Destroy(col.gameObject);         
        }

        //if (col.gameObject.tag == "Enemy_A")
        //{

        //}
    }

    public bool ReadyToDelete()
    {
        return ready_to_delete;
    }
}
