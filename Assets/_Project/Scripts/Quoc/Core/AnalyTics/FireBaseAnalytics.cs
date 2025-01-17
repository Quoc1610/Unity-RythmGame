using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FireBaseAnalytics : iGameAnalytics
{
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        startTime = Time.time;
    }

    public void LogEvent(string name)
    {
        Debug.Log("Log Event");
        Firebase.Analytics.FirebaseAnalytics.LogEvent(name);
    }

    public void LogEvent(string name, Firebase.Analytics.Parameter[] parameters)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(name,parameters);
    }
    public void SetuserProperty(string name, string value)
    {
        Firebase.Analytics.FirebaseAnalytics.SetUserProperty(name,value);
    }
    public void LogEvent(string name, string paraneterName, string parameterValue)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(name,paraneterName,parameterValue);
    }
    
}
