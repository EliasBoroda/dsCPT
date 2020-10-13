
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TaskSettingsManager : MonoBehaviour
{

    public static TaskSettings TaskSettings = new TaskSettings();

    void Awake()
    {
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
    }
}

public class TaskSettings
{
    public string SubjectID;
    public string EventID;
    public string ResponseKey;
    public bool WaitForTrigger;
    public bool Border;
    public bool Big;
    public string TriggerKeyVal = "5";
    public string AbortTrialKeyVal = "x";
    public string StimDuration;
}