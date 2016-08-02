﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FighterSelector : MonoBehaviour {

    public ProfilePicture profilePicture;
    public FighterSelectorButton button;
    public Transform Content;
    public CompareStatsLine[] compareStatsLine;

    public Text userName;
    public Text category;
    public Text heroScore;

    public Text characterCategory;
    public SwitchButton switchButtons;
    public VerticalScrollSnap verticalScrollSnap;


    void Start()
    {
        Events.OnLoadingShow(true);
        Data.Instance.settings.playingTutorial = false;

        Events.OnBackButtonPressed += OnBackButtonPressed;
        Events.SetFighter += SetFighter;

        if (SocialManager.Instance.userData.logged)
        {
            userName.text = Data.Instance.playerSettings.heroData.nick;
            profilePicture.setPicture(SocialManager.Instance.userData.facebookID);
        }
        else
            userName.text = "Anónimo";

        int score = Data.Instance.playerSettings.heroData.stats.score;
        category.text = Categories.GetCategorieByScore(score);
        heroScore.text = "puntos: " + score;
        LoadFighters();    
    }
    void LoadFighters()
    {
        if (Data.Instance.fightersManager.filter == FightersManager.filters.ONLY_FRIENDS && !Data.Instance.fightersManager.FriendsLoaded)
            Data.Instance.fightersManager.LoadFriends(0, 100);
        LoopUntilReady();
    }
    void LoopUntilReady()
    {
        List<PlayerData> fighters;
        if (Data.Instance.fightersManager.filter == FightersManager.filters.ALL)
            fighters = Data.Instance.fightersManager.all;
        else
            fighters = Data.Instance.fightersManager.friends;

        if (fighters.Count < 1)
            Invoke("LoopUntilReady", 1);
        else
            AddFighters(fighters);
    }
    void AddFighters(List<PlayerData> fighters)
    {
        Events.OnLoadingShow(false);
        int id = 0;
        foreach (PlayerData playerData in fighters)
        {
            FighterSelectorButton newButton = Instantiate(button);
            newButton.transform.SetParent(Content);
            newButton.transform.localScale = Vector3.one;
            newButton.Init(id, playerData);
            id++;
        }

        SetFighter(Data.Instance.fightersManager.GetActualFighter());

        int FighterID = (int)(id / 2);

        verticalScrollSnap.Init(FighterID);

        if (Data.Instance.fightersManager.filter == FightersManager.filters.ALL)
            switchButtons.Init(1);
        else
            switchButtons.Init(2);
    }
    void OnDestroy()
    {
        Events.OnBackButtonPressed -= OnBackButtonPressed;
        Events.SetFighter -= SetFighter;
    }
    void OnBackButtonPressed()
    {
        Data.Instance.LoadLevel("03_Home");
    }
    //public void Next(PlayerData playerData)
    //{
    //    //PlayerData playerData = Data.Instance.fightersManager.GetFighter(true);
    //    SetFighter(playerData);
    //}
    //public void Prev(PlayerData playerData)
    //{
    //    //PlayerData playerData = Data.Instance.fightersManager.GetFighter(false);
    //    SetFighter(playerData);
    //}
    void SetFighter(int playerID)
    {
        SetFighter(Data.Instance.fightersManager.GetActualFighters()[playerID]);
    }
    void SetFighter(PlayerData playerData)
    {
        Data.Instance.playerSettings.characterData = playerData;
        int score = playerData.stats.score;
        
        characterCategory.text = Categories.GetCategorieByScore(score).ToUpper();

       PlayerSettings playerSettings = Data.Instance.playerSettings;

       compareStatsLine[0].Init("FUERZA", playerSettings.heroData.stats.Power.ToString(), playerData.stats.Power.ToString());
       compareStatsLine[1].Init("RESISTENCIA", playerSettings.heroData.stats.Resistence.ToString(), playerData.stats.Resistence.ToString());
       compareStatsLine[2].Init("DEFENSA", playerSettings.heroData.stats.Defense.ToString(), playerData.stats.Defense.ToString());
       compareStatsLine[3].Init("VELOCIDAD", playerSettings.heroData.stats.Speed.ToString(), playerData.stats.Speed.ToString());

        string hero_p_g = "";
        string hero_r_g = "";

        if (SocialManager.Instance.userData.logged)
        {
            hero_p_g = playerSettings.heroData.peleas.peleas_g + "/" + playerSettings.heroData.peleas.peleas_p;
            hero_r_g = playerSettings.heroData.peleas.retos_g + "/" + playerSettings.heroData.peleas.retos_p;
        }

        compareStatsLine[4].Init("PELEAS G.", hero_p_g, playerSettings.characterData.peleas.peleas_g + "/" + playerData.peleas.peleas_p);
        compareStatsLine[5].Init("RETOS G.", hero_r_g, playerSettings.characterData.peleas.retos_g + "/" + playerData.peleas.retos_p);
    }
    public void StartGame()
    {
        if (Data.Instance.settings.ToturialReady == 0)
        {
            Data.Instance.settings.playingTutorial = true;
            Data.Instance.LoadLevel("Tutorial");
        }
        else
        {
            Data.Instance.GetComponent<HistorialManager>().LoadHistorial();
            Data.Instance.LoadLevel("04_FightIntro");
        }
    }
    public void PlayTutorial()
    {
        
        Events.OnTutorialReady(0);
        StartGame();
    }

    public void ToggleFighters()
    {
        if (Data.Instance.fightersManager.filter == FightersManager.filters.ONLY_FRIENDS)
            Data.Instance.fightersManager.filter = FightersManager.filters.ALL;
        else
        {
            if (SocialManager.Instance.facebookFriends.all.Count == 0)
            {
                Events.OnGenericPopup("Sin amigos", "No tenés amigos registrados en Combate Space");
                return;
            }
            Data.Instance.fightersManager.SetActivePlayer(0);
            Data.Instance.fightersManager.filter = FightersManager.filters.ONLY_FRIENDS;
        }


        Events.OnLoadingShow(true);
        verticalScrollSnap.Reset();
        LoadFighters();
    }
}
