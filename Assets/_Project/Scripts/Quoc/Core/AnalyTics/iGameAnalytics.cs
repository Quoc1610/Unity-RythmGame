using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iGameAnalytics
{
    void Init();
    void LogEvent(string name);
    void SetuserProperty(string name, string value);
}
