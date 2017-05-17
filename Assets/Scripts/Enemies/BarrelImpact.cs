using UnityEngine;
using System.Collections;

public class BarrelImpact : MonoBehaviour {

    Enemy enemy_script;
    LevelManager level_manager;
    bool score_obtained;

    private AudioManager audio_manager;    

    void Start()
    {
        enemy_script = transform.parent.GetComponent<Enemy>();
        audio_manager = GameObject.FindGameObjectWithTag("Audio_Manager").GetComponent<AudioManager>();
        level_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        score_obtained = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            if (enemy_script.shield_active)
            {
                audio_manager.PlayMetalImpact();
                enemy_script.barrel.GetComponent<Rigidbody>().isKinematic = false;
                enemy_script.shield.SetActive(false);   
                enemy_script.shield_active = false;
            }
            else
            {
                if(!score_obtained)
                {
                    level_manager.UpdateScore(100);
                    score_obtained = true;
                    audio_manager.PlayWoodCrash();
                }
                enemy_script.impacted = true;
                enemy_script.barrel.GetComponent<Rigidbody>().useGravity = true;
            }

            col.gameObject.transform.position = new Vector3(0.0f, -1000.0f, 0.0f);
            Destroy(col.gameObject.GetComponent<SphereCollider>());
            Destroy(col.gameObject.GetComponent<Rigidbody>());
        }
        else if (col.gameObject.tag == "Barrel" && !enemy_script.shield_active)
        {
            if (!score_obtained)
            {
                level_manager.UpdateScore(100);
                score_obtained = true;
                audio_manager.PlayWoodCrash();
            }
            enemy_script.impacted = true;
            enemy_script.barrel.GetComponent<Rigidbody>().useGravity = true;
        }
        else if(col.gameObject.tag == "Item" && !enemy_script.shield_active)
        {
            enemy_script.impacted = true;
            audio_manager.PlayWoodCrash();
            enemy_script.barrel.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
