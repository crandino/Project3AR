using UnityEngine;
using System.Collections;

public class EnemyHit : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            GetComponent<Rigidbody>().useGravity = true;
            Destroy(col.gameObject);                            // Ball disappears when hit a cylinder
        }
    }
}
