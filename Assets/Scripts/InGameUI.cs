﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InGameUI : MonoBehaviour {

    private Image foreground_image;

    private GameObject foreground_go;
    private GameObject background_go;

    private GameObject start_go;

    private GameObject you_win_go;
    private GameObject you_lose_go;

    private GameObject main_menu_go;

    private GameObject balls_marker_go;
    private GameObject balls_marker_background_go;

    private GameObject score_go;
    private GameObject score_background_go;

    private GameObject ui_in_game_bck_1;
    private GameObject ui_in_game_bck_2;

    private GameObject level_manager;
    private GameObject player_cannon;

    private float charging_value;
    private float charging_units;
    private float actual_time_value;

    private int current_balls;

    public void SetCurrentBalls(int balls)
    {
        current_balls = balls;
    }

    private int current_score;

    public void SetCurrentScore(int score)
    {
        current_score = score;
    }

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
        //background_go = GameObject.Find("Canvas").transform.FindChild("Background").gameObject;
        //background_go.SetActive(false);
        foreground_go = GameObject.Find("Canvas").transform.FindChild("Foreground").gameObject;
        foreground_go.SetActive(false);

        //Start Screen
        start_go = GameObject.Find("Canvas").transform.FindChild("StartButton").gameObject;
        start_go.SetActive(false);

        //You Win Screen
        you_win_go = GameObject.Find("Canvas").transform.FindChild("YouWinText").gameObject;
        you_win_go.SetActive(false);

        //You Lose Screen
        you_lose_go = GameObject.Find("Canvas").transform.FindChild("YouLoseText").gameObject;
        you_lose_go.SetActive(false);

        //Main Menu Button
        main_menu_go = GameObject.Find("Canvas").transform.FindChild("MainMenuButton").gameObject;
        main_menu_go.SetActive(false);

        ////Balls Marker
        balls_marker_go = GameObject.Find("Canvas").transform.FindChild("BallsMarker").gameObject;
        balls_marker_go.SetActive(false);
        //balls_marker_background_go = GameObject.Find("Canvas").transform.FindChild("BallsMarkerBackground").gameObject;
        //balls_marker_background_go.SetActive(false);

        ////Score Marker
        score_go = GameObject.Find("Canvas").transform.FindChild("ScoreText").gameObject;
        score_go.SetActive(false);
        //score_background_go = GameObject.Find("Canvas").transform.FindChild("ScoreBackground").gameObject;
        //score_background_go.SetActive(false);

        //UI in game background
        ui_in_game_bck_1 = GameObject.Find("Canvas").transform.FindChild("ARGameUI1").gameObject;
        ui_in_game_bck_1.SetActive(false);
        ui_in_game_bck_2 = GameObject.Find("Canvas").transform.FindChild("ARGameUI2").gameObject;
        ui_in_game_bck_2.SetActive(false);

        level_manager = GameObject.FindGameObjectWithTag("GameController");
        player_cannon = GameObject.FindGameObjectWithTag("Cannon");

        game_start = false;
        main_menu = false;
    }

    // Update is called once per frame
    void Update()
    {
        game_phase = level_manager.GetComponent<LevelManager>().GetCurrentPhase();

        switch (game_phase)
        {
            case (GAME_PHASES.MENU):
                {
                    if (game_on == true)
                    {
                        game_start = false;
                        main_menu = false;

                        you_win_go.SetActive(false);
                        you_lose_go.SetActive(false);
                        main_menu_go.SetActive(false);

                        game_on = false;
                    }

                    start_go.SetActive(true);
                               
                    break;
                }

            case (GAME_PHASES.GAME):
                {
                    if (game_on == false)
                    {
                        start_go.SetActive(false);

                        ui_in_game_bck_1.SetActive(true);
                        ui_in_game_bck_2.SetActive(true);

                        //background_go.SetActive(true);
                        foreground_go.SetActive(true);

                        balls_marker_go.SetActive(true);
                        //balls_marker_background_go.SetActive(true);

                        score_go.SetActive(true);
                        //score_background_go.SetActive(true);

                        game_on = true;
                    }

                    actual_time_value = player_cannon.GetComponent<TurretController>().final_time;
                    charging_bar_value = (int)(actual_time_value / charging_units);

                    if (current_balls >= 0)
                    {
                        balls_marker_go.GetComponent<Text>().text = current_balls.ToString();
                    }
                    score_go.GetComponent<Text>().text = current_score.ToString();
                }
                break;

            case (GAME_PHASES.WIN):
                {
                    ui_in_game_bck_1.SetActive(false);
                    ui_in_game_bck_2.SetActive(false);

                    //background_go.SetActive(false);
                    foreground_go.SetActive(false);

                    balls_marker_go.SetActive(false);
                    //balls_marker_background_go.SetActive(false);

                    score_go.SetActive(false);
                    //score_background_go.SetActive(false);

                    you_win_go.SetActive(true);
                    main_menu_go.SetActive(true);

                    break;
                }

            case (GAME_PHASES.LOSE):
                {
                    ui_in_game_bck_1.SetActive(false);
                    ui_in_game_bck_2.SetActive(false);

                    //background_go.SetActive(false);
                    foreground_go.SetActive(false);

                    balls_marker_go.SetActive(false);
                    //balls_marker_background_go.SetActive(false);

                    score_go.SetActive(false);
                    //score_background_go.SetActive(false);

                    you_lose_go.SetActive(true);
                    main_menu_go.SetActive(true);

                    break;
                }
        }
    }

}
