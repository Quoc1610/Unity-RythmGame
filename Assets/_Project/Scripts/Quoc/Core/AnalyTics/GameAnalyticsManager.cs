using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using Unity.VisualScripting;
using UnityEngine;

public class GameAnalyticsManager : SingletonMono<GameAnalyticsManager>
{
    private FireBaseAnalytics firebaseAnalytics;
    
    // Start is called before the first frame update
    void Awake()
    {
            DontDestroyOnLoad(this);
            firebaseAnalytics = new FireBaseAnalytics();
            firebaseAnalytics.Init();
    }

    public void SetUserProperties(string name, string value)
    {
        firebaseAnalytics.SetuserProperty(name,value);
    }

    public void LogEvent(string name)
    {
        firebaseAnalytics.LogEvent(name);
    }
    public void LogEvent(string name,Firebase.Analytics.Parameter[] parameters)
    {
        firebaseAnalytics.LogEvent(name,parameters);
    }

    public void LogEvent(string name, string parameterName, string parametervalue)
    {
        firebaseAnalytics.LogEvent(name,parameterName,parametervalue);
    }

    public void PlayEnd(string nameSong, string result, int score, int miss, float timeRemain)
    {
        Firebase.Analytics.Parameter[] parameters =
        {
            new Parameter("nameSong", nameSong),
            new Parameter("result", result),
            new Parameter("score", score),
            new Parameter("miss", miss),
            new Parameter("timeRemain", timeRemain)

        };
        LogEvent("PlayEnd",parameters);
    }

    public void PlayStart(string nameSong)
    {
        LogEvent("PlayStart","nameSong",nameSong);
    }
}
