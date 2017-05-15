using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InGameUI : MonoBehaviour {

    Image foreground_image;

    private float charging_value;
    private float charging_units;
    private float actual_time_value;

    private GAME_PHASES game_phase;
    private bool game_on;

    [HideInInspector] public bool game_start;
    [HideInInspector] public bool main_menu;

    public void SetStartGame(bool start)
    {
        game_start = start;
    }

    public void SetMainMenu(bool menu)
    {
        main_menu = menu;
    }

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
        game_on = false;

        foreground_image = GameObject.Find("Canvas").transform.FindChild("Foreground").GetComponent<Image>();
        charging_bar_value = 0;
        charging_value = GameObject.FindGameObjectWithTag("Cannon").GetComponent<TurretController>().charger_time;
        charging_units = charging_value / 100.0f;

        //Charging bar
        GameObject.Find("Canvas").transform.FindChild("Background").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.FindChild("Foreground").gameObject.SetActive(false);

        //Start Screen
        GameObject.Find("Canvas").transform.FindChild("StartButton").gameObject.SetActive(false);

        //You Win Screen
        GameObject.Find("Canvas").transform.FindChild("YouWinText").gameObject.SetActive(false);

        //You Lose Screen
        GameObject.Find("Canvas").transform.FindChild("YouLoseText").gameObject.SetActive(false);

        //Main Menu Button
        GameObject.Find("Canvas").transform.FindChild("MainMenuButton").gameObject.SetActive(false);

        game_start = false;
        main_menu = false;
    }

    // Update is called once per frame
    void Update()
    {
        game_phase = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().GetCurrentPhase();

        switch (game_phase)
        {
            case (GAME_PHASES.MENU):
                {
                    if (game_on == true)
                    {
                        game_start = false;
                        main_menu = false;

                        GameObject.Find("Canvas").transform.FindChild("YouWinText").gameObject.SetActive(false);
                        GameObject.Find("Canvas").transform.FindChild("YouLoseText").gameObject.SetActive(false);
                        GameObject.Find("Canvas").transform.FindChild("MainMenuButton").gameObject.SetActive(false);

                        game_on = false;
                    }

                    GameObject.Find("Canvas").transform.FindChild("StartButton").gameObject.SetActive(true);
                    break;
                }

            case (GAME_PHASES.GAME):
                {
                    if (game_on == false)
                    {
                        GameObject.Find("Canvas").transform.FindChild("StartButton").gameObject.SetActive(false);

                        GameObject.Find("Canvas").transform.FindChild("Background").gameObject.SetActive(true);
                        GameObject.Find("Canvas").transform.FindChild("Foreground").gameObject.SetActive(true);
                        game_on = true;
                    }

                    actual_time_value = GameObject.FindGameObjectWithTag("Cannon").GetComponent<TurretController>().final_time;
                    charging_bar_value = (int)(actual_time_value / charging_units);
                }
                break;

            case (GAME_PHASES.WIN):
                {
                    GameObject.Find("Canvas").transform.FindChild("Background").gameObject.SetActive(false);
                    GameObject.Find("Canvas").transform.FindChild("Foreground").gameObject.SetActive(false);

                    GameObject.Find("Canvas").transform.FindChild("YouWinText").gameObject.SetActive(true);
                    GameObject.Find("Canvas").transform.FindChild("MainMenuButton").gameObject.SetActive(true);
                    break;
                }

            case (GAME_PHASES.LOSE):
                {
                    GameObject.Find("Canvas").transform.FindChild("Background").gameObject.SetActive(false);
                    GameObject.Find("Canvas").transform.FindChild("Foreground").gameObject.SetActive(false);

                    GameObject.Find("Canvas").transform.FindChild("YouLoseText").gameObject.SetActive(true);
                    GameObject.Find("Canvas").transform.FindChild("MainMenuButton").gameObject.SetActive(true);
                    break;
                }
        }
    }

}
