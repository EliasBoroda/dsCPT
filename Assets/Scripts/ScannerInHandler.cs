using UnityEngine;
using System;
using Assets.Scripts;
using System.Collections;

public class ScannerInHandler : MonoBehaviour {

    private TaskEngine _taskEngine;
    private ConfigureViewModel _configView;

    //do I need a new instance or log to the instance that is created in task engine??
    //private IPerfLog _logger = new FileSystemLogger();

    Sprite[] _waitingSprites;

    private bool _trigger = false;

    private int _scanCount;
    private DateTime _scanTime = DateTime.Now;

    // Use this for initialization
    void Awake ()
    {
        _taskEngine = FindObjectOfType<TaskEngine>();
        _configView = FindObjectOfType<ConfigureViewModel>();

        _waitingSprites = Resources.LoadAll<Sprite>("Sprites/Others");
        _trigger = Input.GetKeyDown(TaskSettingsManager.TaskSettings.TriggerKeyVal);
    }
	
    public void WaitForTrigger()
    {
        if (!_trigger && _taskEngine.CurrentTaskState == TaskState.WaitForTrigger)
        {
            _taskEngine.MainGO.GetComponent<SpriteRenderer>().sprite = _waitingSprites[1];
            _taskEngine.MainGO.SetActive(true);
        }

        if (_trigger && _taskEngine.CurrentTaskState == TaskState.WaitForTrigger)
        {
            _taskEngine.MainGO.SetActive(false);
            _taskEngine.CurrentTaskState = TaskState.RunTask;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        _trigger = Input.GetKeyDown(TaskSettingsManager.TaskSettings.TriggerKeyVal);

        if (_trigger)
        {
            _scanCount++;
            _scanTime = DateTime.Now;

            string _sT = _scanTime.ToString("yyyyMMdd-HHmmss.fff");

            _taskEngine._logger.LogScan(_scanCount + " , " + _sT);
        }
    }
}
