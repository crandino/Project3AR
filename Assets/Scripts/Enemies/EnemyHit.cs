using UnityEngine;
using System.Collections;

public class EnemyHit : MonoBehaviour {

    Enemy enemy_script;

    void Start()
    {
        enemy_script = transform.parent.GetComponent<Enemy>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            if (enemy_script.shield_active)
            {
                enemy_script.PlayMetalImpactSound();
                enemy_script.shield_active = false;
            }
            else
                enemy_script.impacted = true;


            col.gameObject.transform.position = new Vector3(0.0f, -1000.0f, 0.0f);
            Destroy(col.gameObject.GetComponent<SphereCollider>());
            Destroy(col.gameObject.GetComponent<Rigidbody>());
        }

        if (col.gameObject.tag == "Enemy")
        {
            if(!enemy_script.shield_active)
            {
                enemy_script.impacted = true;
            }
        }

    }
}
