using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{
    public GameObject ball_prefab;
    public float power_launch;

    private GameObject plane;

	// Use this for initialization
	void Start ()
    { }

    // Update is called once per frame
    void Update()
    {
        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                Vector3 ball_position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f, gameObject.transform.position.z);
                GameObject ball_instance = (GameObject)Instantiate(ball_prefab, ball_position, Quaternion.identity);
                ball_instance.GetComponent<Rigidbody>().AddForce(power_launch * gameObject.transform.forward);
            }
        }
    }
}
