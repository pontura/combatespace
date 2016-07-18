﻿using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class DataController : MonoBehaviour
{
    private string secretKey = "ranlogic2008";
    const string URL = "http://www.pontura.com/combateSpace/";
    private string getUserIdByFacebookID_URL = URL + "getUserIdByFacebookID.php?";
    private string createUser_URL = URL + "createUser.php?";
    private string updateUser_URL = URL + "updateUser.php?";
    private string updatePeleas_URL = URL + "updatePeleas.php?";
    private string getUsersByScore_URL = URL + "getUsersByScore.php?";
    private string saveStats_URL = URL + "saveStats.php?";
    private string saveNewPelea_URL = URL + "saveNewPelea.php?";
    private string getPeleas_URL = URL + "getPeleasByFacebookID.php?";
    private string getHistorialPeleas_URL = URL + "getHistorialPeleas.php?";
    
    void Start()
    {        
        Events.OnUpdatePlayerData += OnUpdatePlayerData;
        Events.OnSavePelea += OnSavePelea;
        Events.OnSaveStats += OnSaveStats;
        Events.OnSaveNewPelea += OnSaveNewPelea;

        SocialEvents.OnFacebookLogin += OnFacebookLogin;
        SocialEvents.OnGetUsersByScore += OnGetUsersByScore;
        SocialEvents.OnGetFights += OnGetFights;
        SocialEvents.OnGetHistorial += OnGetHistorial;
    }
    void OnFacebookLogin(string facebookID, string username, string email)
    {
        print("onfaecbooklogin");
        StartCoroutine(CheckIfUserExistsOnLocalDB(facebookID, username, email));
    }
    IEnumerator CheckIfUserExistsOnLocalDB(string _facebookID, string _username, string _email)
    {
        if (_facebookID == "")
            yield break;

        string post_url = getUserIdByFacebookID_URL + "facebookID=" + _facebookID;

        print(post_url);

        WWW receivedData = new WWW(post_url);
        yield return receivedData;
        if (receivedData.error != null)
            print("There was an error in CheckIfUserExistsOnLocalDB: " + receivedData.error);
        else
        {
            try
            {
                string[] userData = Regex.Split(receivedData.text, ":");
                int userID = System.Int32.Parse(userData[1]);
                string username = userData[2];

                Data.Instance.playerSettings.heroData.stats.Power = System.Int32.Parse(userData[3]);
                Data.Instance.playerSettings.heroData.stats.Resistence = System.Int32.Parse(userData[4]);
                Data.Instance.playerSettings.heroData.stats.Defense = System.Int32.Parse(userData[5]);
                Data.Instance.playerSettings.heroData.stats.Speed = System.Int32.Parse(userData[6]);
                Data.Instance.playerSettings.heroData.stats.score = System.Int32.Parse(userData[7]);
                
                Data.Instance.playerSettings.heroData.peleas.peleas_g = System.Int32.Parse(userData[8]);
                Data.Instance.playerSettings.heroData.peleas.peleas_p = System.Int32.Parse(userData[9]);
                Data.Instance.playerSettings.heroData.peleas.retos_g = System.Int32.Parse(userData[10]);
                Data.Instance.playerSettings.heroData.peleas.retos_p = System.Int32.Parse(userData[11]);

                string nick = userData[12];

                SocialEvents.OnUserReady(_facebookID, _username, nick);
            }
            catch
            {
                Debug.Log("new user!");
                CreateUser(_facebookID, _username, _email);
            }
        }
    }
    public void CreateUser(string _facebookID, string _username, string _email)
    {
        Stats stats = new Stats();
        stats.Power = 10;  stats.Resistence = 10; stats.Defense = 10;  stats.Speed = 10;
        Data.Instance.playerSettings.heroData.stats = stats;

        StartCoroutine(CreateUserRoutine(_facebookID, _username, _email));
    }
    IEnumerator CreateUserRoutine(string _facebookID, string _username, string _email)
    {
       // username = username.Replace(" ", "_");
        string hash = Md5Test.Md5Sum(_facebookID + _username  + secretKey);
        string post_url = createUser_URL + "username=" + WWW.EscapeURL(_username) + "&facebookID=" + WWW.EscapeURL(_facebookID) + "&hash=" + hash;
        print("CreateUser : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null)
            print("No pudo crear el nuevo user: " + hs_post.error);
        else
        {
            print("user agregado: " + hs_post.text);
            int userId = int.Parse(hs_post.text);
            SocialEvents.OnUserReady(_facebookID, _username, _email);
        }
    }

    void OnUpdatePlayerData(PlayerData playerData)
    {
        StartCoroutine(UpdateUserRoutine( playerData ));
    }
    IEnumerator UpdateUserRoutine(PlayerData playerData)
    {
       // username = username.Replace(" ", "_");
        string hash = playerData.facebookID;

        if (playerData.nick != "")
            hash += playerData.nick;

        hash = Md5Test.Md5Sum(hash + secretKey);
        string post_url = updateUser_URL + "nick=" + WWW.EscapeURL(playerData.nick) + "&facebookID=" + playerData.facebookID + "&hash=" + hash;
        print("UpdateUser : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null)
            print("Error haciendo update de user");
        else
        {
            print("user updateado: " + hs_post.text);

        }
    }




    void OnSavePelea(string facebookID, Peleas peleas)
    {
        StartCoroutine(OnUpdatePeleasRoutine(facebookID, peleas.peleas_g, peleas.peleas_p, peleas.retos_g, peleas.retos_p));
    }
    IEnumerator OnUpdatePeleasRoutine(string facebookID, int peleas_g, int peleas_p, int retos_g, int retos_p)
    {
        string hash = facebookID + peleas_g + peleas_p + retos_g + retos_p;
        hash = Md5Test.Md5Sum(hash + secretKey);
        string post_url = updatePeleas_URL + "facebookID=" + facebookID + "&peleas_g=" + peleas_g + "&peleas_p=" + peleas_p + "&retos_g=" + retos_g + "&retos_p=" + retos_p + "&hash=" + hash;
        print("OnUpdatePeleas : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null) print("Error haciendo update de user"); else{ print("user updateado: " + hs_post.text); }
    }


    private System.Action<string> OnGetUsersByScoreListener;
    public void OnGetUsersByScore(System.Action<string> OnGetUsersByScoreListener, int min, int max)
    {
        this.OnGetUsersByScoreListener = OnGetUsersByScoreListener;
        StartCoroutine(GetUsersByScoreRoutine(min, max));
    }
    IEnumerator GetUsersByScoreRoutine(int min, int max)
    {
        string post_url = getUsersByScore_URL + "min=" + min + "&max=" + max;
        print("OnGetUsersByScore : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null) print("Error con: GetUsersByScoreRoutine: " + hs_post.error); else {OnGetUsersByScoreListener(hs_post.text);}
    }
    private void OnSaveNewPelea(Fight fight)
    {
        string hash = fight.retador_facebookID + fight.retado_facebookID + fight.winner;
        hash = Md5Test.Md5Sum(hash + secretKey);
        string post_url = saveNewPelea_URL + "retador_username=" + WWW.EscapeURL(fight.retador_username) + "&retado_username=" + WWW.EscapeURL(fight.retado_username) + "&retador_facebookID=" + fight.retador_facebookID + "&retado_facebookID=" + fight.retado_facebookID + "&winner=" + fight.winner + "&hash=" + hash;
        print("OnSaveNewPelea : " + post_url);
        WWW hs_post = new WWW(post_url);
    }

    public void OnSaveStats(Stats stats)
    {
        int stat1 = stats.Power;
        int stat2 = stats.Resistence;
        int stat3 = stats.Defense;
        int stat4 = stats.Speed;
        int score = stats.score;

        string facebookID = SocialManager.Instance.userData.facebookID;
        string hash = facebookID + score + stat1 + stat2 + stat3 + stat4;
        hash = Md5Test.Md5Sum(hash + secretKey);
        string post_url = saveStats_URL + "facebookID=" + facebookID + "&score=" + score + "&stat1=" + stat1 + "&stat2=" + stat2 + "&stat3=" + stat3 + "&stat4=" + stat4 + "&hash=" + hash;
        print("OnSaveStats : " + post_url);
        WWW hs_post = new WWW(post_url);
    }
    private System.Action<string> OnGetFightsListener;
    public void OnGetFights(System.Action<string> OnGetFightsListener)
    {
        this.OnGetFightsListener = OnGetFightsListener;
        StartCoroutine(OnGetFightsRoutine());
    }
    IEnumerator OnGetFightsRoutine()
    {
        string facebookID = SocialManager.Instance.userData.facebookID;
        string post_url = getPeleas_URL + "facebookID=" + facebookID;
        print("OnGetFightsRoutine : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null) print("Error con: OnGetFightsRoutine: " + hs_post.error); else { OnGetFightsListener(hs_post.text); }
    }

    private System.Action<string> OnGetHistorialListener;
    public void OnGetHistorial(System.Action<string> OnGetHistorialListener, string facebookID, string facebookID2)
    {
        this.OnGetHistorialListener = OnGetHistorialListener;
        StartCoroutine(OnGetHistorial(facebookID, facebookID2));
    }
    IEnumerator OnGetHistorial(string facebookID, string facebookID2)
    {
        string post_url = getHistorialPeleas_URL + "facebookID=" + facebookID + "&facebookID2=" + facebookID2;
        print("OnGetHistorial : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null) print("Error con: OnGetHistorial: " + hs_post.error); else { OnGetHistorialListener(hs_post.text); }
    }


}

