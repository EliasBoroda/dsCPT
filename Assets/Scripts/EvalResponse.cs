
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class EvalResponse : MonoBehaviour {

    private TaskEngine _taskEngine;
    public ResponseEval CurrentResponseEval = ResponseEval.Init;

    public double RTstamp;//time stamp for when response was recieved 
    public int ResponseTime;
    private int _hitFlag = 0;
    private int _falseAlarmFlag = 0;
    private int _timeErrorFlag = 0;
    public int TimeErrorCount = 0;
    public int FalseAlarmCount = 0;
    public int CorrectRejectCount = 0;
    public int MissCount = 0;
    public int IncorrectResponseCount = 0;
    public int HitCount = 0;

    private List<int> _responseTimes = new List<int>();

    // Use this for initialization
    void Awake ()
    {
        _taskEngine = FindObjectOfType<TaskEngine>();
        CurrentResponseEval = ResponseEval.Init;
    }

    public void EvaluateReponse()
    {
        //Time stamp taken when button is pressed
        RTstamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
        //calculate Response Time
        ResponseTime = Convert.ToInt32(RTstamp - _taskEngine.StimTime);
        //saving reponse times
        _responseTimes.Add(ResponseTime);

        switch (_taskEngine.CurrentBlockType)
        {
            #region TrialBlock
            //respond to zeros
            case BlockType.TrialBlock:

                //stimulus constraint 
                if (_taskEngine.StimCode == 0)
                {
                    CurrentResponseEval = ResponseEval.HIT;
                    _hitFlag = 1;
                }
                else if (_taskEngine.StimCode != 0)
                {
                    CurrentResponseEval = ResponseEval.FAL;
                    _falseAlarmFlag = 1;
                }

                break;
                #endregion
        }

        if (_hitFlag == 1)
        {
            HitCount++;
            _hitFlag = 0;
        }

        if (_falseAlarmFlag == 1)
        {
            FalseAlarmCount++;
            _falseAlarmFlag = 0;
        }

        if (_timeErrorFlag == 1)
        {
            TimeErrorCount++;
            _timeErrorFlag = 0;
        }

        Debug.Log(_taskEngine.StimCode + " " + CurrentResponseEval + "  " + ResponseTime);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}

public enum ResponseEval
{
    HIT,
    FAL,
    CRJ,
    MISS,
    Init
}
