﻿using UnityEngine;
using System.Collections;

public class GameSettingsPopup : MonoBehaviour
{
    public GameObject canvas;
    public GameObject panel;
    public SwitchButton modeSwitchButton;

    void Start()
    {
        panel.SetActive(false);
        Events.OnGameSettings += OnGameSettings;
    }
    void OnDestroy()
    {
        Events.OnGameSettings -= OnGameSettings;
    }
    void OnGameSettings()
    {
        panel.SetActive(true);
        canvas.SetActive(true);
        Time.timeScale = 0;
        SetActive();
    }
    public void SwitchMode()
    {
        if (Data.Instance.playerSettings.control == PlayerSettings.controls.CONTROL_360)
            Mode_Volante();
        else
            Mode_360();
    }
    public void Mode_360()
    {
        Data.Instance.playerSettings.control = PlayerSettings.controls.CONTROL_360;
        Data.Instance.LoadLevel("03_Home");
        SetActive();
    }
    public void Mode_Volante()
    {
        Data.Instance.playerSettings.control = PlayerSettings.controls.CONTROL_JOYSTICK;
        Data.Instance.LoadLevel("03_Home");
        SetActive();
    }
    public void ResetApp()
    {
        PlayerPrefs.DeleteAll();
        SocialEvents.ResetApp();
        Data.Instance.LoadLevel("03_Home");
    }
    public void Close()
    {
        panel.SetActive(false);
        canvas.SetActive(false);
        Time.timeScale = 1;
    }
    void SetActive()
    {
        if (Data.Instance.playerSettings.control == PlayerSettings.controls.CONTROL_360)
        {
            modeSwitchButton.Init(2);
        }
        else
        {
            modeSwitchButton.Init(1);
        }
    }
}