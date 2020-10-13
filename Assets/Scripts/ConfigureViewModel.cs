using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class ConfigureViewModel : MonoBehaviour {

    public GameObject MainGO;
    public GameObject ConfigForm;

    public InputField ResponseKey;
    public Toggle WaitForTrigger;
    public InputField AbortTrialKeyVal;
    public InputField SubjectID;
    public InputField EventID;
    public InputField StimDuration;
    public Toggle BorderSprites;
    public Toggle BigSprites;

    public void Awake()
    {
        var responseKey = PlayerPrefs.GetString("ResponseKey");
        if (!string.IsNullOrEmpty(responseKey))
            ResponseKey.text = responseKey;
        else
            ResponseKey.text = "4";

        var abortTrialKeyVal = PlayerPrefs.GetString("AbortTrialKeyVal");
        if (!string.IsNullOrEmpty(abortTrialKeyVal))
            AbortTrialKeyVal.text = abortTrialKeyVal;
        else
            AbortTrialKeyVal.text = "x";

        var subjectID= PlayerPrefs.GetString("SubjectID");
        if (!string.IsNullOrEmpty(subjectID))
            SubjectID.text = subjectID;
        else
            SubjectID.text = "Null";

        var eventID = PlayerPrefs.GetString("EventID");
        if (!string.IsNullOrEmpty(eventID))
            EventID.text = eventID;
        else
            EventID.text = "Null";

        var stimDuration = PlayerPrefs.GetString("StimDuration");
        if (!string.IsNullOrEmpty(stimDuration))
            StimDuration.text = stimDuration;
        else
            StimDuration.text = "0.0333";

    }


    public void Run()
    {
        Debug.Log("Run Called");
        SceneManager.LoadScene("TaskMain");

        Destroy(ConfigForm);
    }

    public void SetSubjectID(string subjectID)
    {
        TaskSettingsManager.TaskSettings.SubjectID = subjectID;
        PlayerPrefs.SetString("SubjectID", subjectID);
    }

    public void SetEventID(string eventID)
    {
        TaskSettingsManager.TaskSettings.EventID = eventID;
        PlayerPrefs.SetString("EventID", eventID);
    }

    public void SetResponseKey(string responseKeyVal)
    {
        TaskSettingsManager.TaskSettings.ResponseKey = responseKeyVal;
        PlayerPrefs.SetString("ResponseKey", responseKeyVal);
    }

    public void EnableTriggerOnScanner(bool val)
    {
        TaskSettingsManager.TaskSettings.WaitForTrigger = val;
    }

    public void SetTriggerKeyVal(string triggerKeyVal)
    {
        TaskSettingsManager.TaskSettings.ResponseKey = triggerKeyVal;
        PlayerPrefs.SetString("TriggerKeyVal", triggerKeyVal);
    }

    public void SetAbortTrialKeyVal(string abortTrialKeyVal)
    {
        TaskSettingsManager.TaskSettings.AbortTrialKeyVal = abortTrialKeyVal;
        PlayerPrefs.SetString("AbortTrialKeyVal", abortTrialKeyVal);
    }

    public void SetStimDuration(string stimDuration)
    {
        TaskSettingsManager.TaskSettings.StimDuration = stimDuration;
        PlayerPrefs.SetString("StimDuration", stimDuration);
    }

    public void Abort()
    {
        Debug.Log("Aborting Trial...Return to Config");
        SceneManager.LoadScene("ConfigureScreen");
    }

    public void Return()
    {
        Debug.Log("Returning to Config");
        SceneManager.LoadScene("ConfigureScreen");
    }

    public void UseBorderSprites(bool val)
    {
        TaskSettingsManager.TaskSettings.Border = val;
    }

    public void UseBigSprites(bool val)
    {
        TaskSettingsManager.TaskSettings.Big = val;
    }
}
