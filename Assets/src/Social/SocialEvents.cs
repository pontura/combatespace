﻿using UnityEngine;
using System.Collections;
using System;

public static class SocialEvents {

    public static System.Action<System.Action<string, string>, string> OnGetNewFighter = delegate { };
    public static System.Action<System.Action<string>, string, string> OnGetHistorial = delegate { };
    public static System.Action<System.Action<string>, int, int, bool> OnGetUsersByScore = delegate { };
    public static System.Action<System.Action<string>> OnGetFights = delegate { };
    

    public static System.Action<bool> OnUserExistInDB = delegate { };
    public static System.Action OnUserCreatedInDB = delegate { };
    public static System.Action ResetApp = delegate { };

    //facebookID, id
    public static System.Action<string, string, string> OnUserReady = delegate { };
    public static System.Action OnFacebookLoginPressed = delegate { };
    public static System.Action OnFacebookFriends = delegate { };
    public static System.Action OnFacebookInviteFriends = delegate { };
    public static System.Action OnFacebookError = delegate { };

    public static System.Action OnFacebookLogout = delegate { };
    public static System.Action<string, string, string> OnFacebookLogin = delegate { };
    public static System.Action<string, string> AddFacebookFriend = delegate { }; 

    //Hiscores:
    public static System.Action<int> OnNewHiscore = delegate { };
    public static System.Action<int> OnAddToTotalScore = delegate { };
    public static System.Action<int> OnSetToTotalBarScore = delegate { };

    //metrics
    public static System.Action<string> OnMetricState = delegate { };
    public static System.Action<string> OnMetricAction = delegate { };
    public static System.Action<string, string> OnMetricActionSpecial = delegate { };

    public static System.Action OnRefreshRanking = delegate { };

    //challenges:
    //facebookID, op_facebookID, score
    public static System.Action<string, string, int> OnChallengeCreate = delegate { };
    public static System.Action<string> OnChallengeDelete = delegate { };
    public static System.Action OnChallengesLoad = delegate { };
    public static System.Action<string, string> OnChallengeRemind = delegate { };
    public static System.Action<string, string, string, float> OnChallengeClose = delegate { };
    public static System.Action<string> OnChallengeNotificated = delegate { };
    public static System.Action<string, string, string, float> OnChallengeConfirm = delegate { };
}
