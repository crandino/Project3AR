using UnityEngine;
using System.Collections;

public class BarrelImpact : MonoBehaviour {

    Enemy enemy_script;

    void Start()
    {
        enemy_script = transform.parent.GetComponent<Enemy>();
    }

    void OnCollisionEnter(Collision col)
    {
        // Colisions between ball and enemy
        if (col.gameObject.tag == "Ball")
        {
            if (enemy_script.shield_active)
            {
                enemy_script.PlayMetalImpactSound();
                enemy_script.shield_active = false;
            }
            else
            {
                enemy_script.impacted = true;
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
            if (!enemy_script.shield_active)
            {
                enemy_script.impacted = true;
            }
        }
    }
}
