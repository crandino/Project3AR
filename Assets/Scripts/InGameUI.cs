using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InGameUI : MonoBehaviour {

    Image foreground_image;

    private float charging_value;
    private float charging_units;
    private float actual_time_value;

    private int charging_bar_value
    {
        //Getter for charging_bar_value
        get
        {
            if (foreground_image != null)
            {
                return (int)(foreground_image.fillAmount * 100.0f);
            }
            else
            {
                return 0;
            }
        }

        //Setter for charging_bar_value
        set
        {
            if (foreground_image != null)
            {
                foreground_image.fillAmount = value / 100.0f;
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        foreground_image = GameObject.Find("Canvas").transform.FindChild("Foreground").GetComponent<Image>();
        charging_bar_value = 0;
        charging_value  = GameObject.FindGameObjectWithTag("Cannon").GetComponent<TurretController>().charger_time;
        charging_units = charging_value / 100.0f;
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        actual_time_value = GameObject.FindGameObjectWithTag("Cannon").GetComponent<TurretController>().final_time;
        charging_bar_value = (int)(actual_time_value / charging_units);
    }

}
