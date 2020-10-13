
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

//should rename this to KeyboardHandler
public class KeyboardHandler : MonoBehaviour
{

    private TaskEngine _taskEngine;
    private ConfigureViewModel _configView;
    private EvalResponse _evalResponse;

    public bool Response;

    // Use this for initialization
    void Awake()
    {
        _taskEngine = FindObjectOfType<TaskEngine>();
        _configView = FindObjectOfType<ConfigureViewModel>();
        _evalResponse = FindObjectOfType<EvalResponse>();
    }

    void ProcessKeystroke()
    {
        Response = Input.GetKeyDown(TaskSettingsManager.TaskSettings.ResponseKey);
        var abortTrial = Input.GetKeyDown(TaskSettingsManager.TaskSettings.AbortTrialKeyVal);

        try
        {
            if (Response)
            {
                if (_taskEngine.CurrentBlockType == BlockType.TrialBlock)
                {
                    //if (_taskEngine.CurrentResponseState == ResponseWindow.Open)
                    //{
                        _evalResponse.EvaluateReponse();
                    //    _taskEngine.CurrentResponseState = ResponseWindow.Closed;
                    //}
                }
            }

            if (abortTrial)
            {
                _taskEngine.AbortTrial();
                _configView.Abort();
            }
        }

        catch (System.Exception e)
        {
            int i = 0;
            Debug.Log(e);
        }

    }

    // Update is called once per frame
    void Update()
    {
        ProcessKeystroke();
    }
}









//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using Random = UnityEngine.Random;
//using UnityEngine.SceneManagement;

////should rename this to KeyboardHandler
//public class ResponseHandler : MonoBehaviour
//{

//    private TaskEngine _taskEngine;
//    private ConfigureViewModel _configView;
//    public ResponseEval ResponseEval = ResponseEval.Init;

//    public double RTstamp;//time stamp for when response was recieved 
//    public int ResponseTime;
//    private int _hitFlag = 0;
//    private int _falseAlarmFlag = 0;
//    private int _timeErrorFlag = 0;
//    public int TimeErrorCount = 0;
//    public int FalseAlarmCount = 0;
//    public int CorrectRejectCount = 0;
//    public int MissCount = 0;
//    public int IncorrectResponseCount = 0;
//    public int HitCount = 0;
//    //private int _rwLowBound = 150;
//    //private int _rwHiBound = 1220;

//    public bool Response;

//    private List<int> _responseTimes = new List<int>();

//    // Use this for initialization
//    void Start()
//    {
//        _taskEngine = FindObjectOfType<TaskEngine>();
//        _configView = FindObjectOfType<ConfigureViewModel>();

//        ResponseEval = ResponseEval.Init;
//    }

//    void ProcessKeystroke()
//    {
//        Response = Input.GetKeyDown(TaskSettingsManager.TaskSettings.ResponseKey);

//        try
//        {
//            //var Response = Input.GetKeyDown(TaskSettingsManager.TaskSettings.ResponseKey);
//            if (Response)
//            {
//                //Time stamp taken when button is pressed
//                RTstamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
//                //calculate Response Time
//                ResponseTime = Convert.ToInt32(RTstamp - _taskEngine.StimTime);
//                //saving reponse times
//                _responseTimes.Add(ResponseTime);

//                switch (_taskEngine.CurrentBlockType)
//                {
//                    #region TrialBlock
//                    //respond to zeros
//                    case BlockType.TrialBlock:

//                        //stimulus constraint 
//                        if (_taskEngine.StimCode == 0)
//                        {
//                            ResponseEval = ResponseEval.HIT;
//                            _hitFlag = 1;
//                        }
//                        else if (_taskEngine.StimCode != 0)
//                        {
//                            ResponseEval = ResponseEval.FAL;
//                            _falseAlarmFlag = 1;
//                        }

//                        break;
//                        #endregion
//                }

//                if (_hitFlag == 1)
//                {
//                    HitCount++;
//                    _hitFlag = 0;
//                }

//                if (_falseAlarmFlag == 1)
//                {
//                    FalseAlarmCount++;
//                    _falseAlarmFlag = 0;
//                }

//                if (_timeErrorFlag == 1)
//                {
//                    TimeErrorCount++;
//                    _timeErrorFlag = 0;
//                }

//                Debug.Log("correct count " + HitCount.ToString() + " INcorrect count " + IncorrectResponseCount.ToString() + " " + _taskEngine.StimCode + " " + ResponseTime);

//            }

//            var abortTrial = Input.GetKeyDown(TaskSettingsManager.TaskSettings.AbortTrialKeyVal);
//            if (abortTrial)
//            {
//                _taskEngine.AbortTrial();
//                _configView.Abort();
//            }
//        }

//        catch (System.Exception e)
//        {
//            int i = 0;
//            Debug.Log(e);
//        }

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (_taskEngine.CurrentBlockType == BlockType.TrialBlock)
//        {
//            ProcessKeystroke();
//        }
//    }
//}

//public enum ResponseEval
//{
//    HIT,
//    FAL,
//    CRJ,
//    MISS,
//    Init
//}