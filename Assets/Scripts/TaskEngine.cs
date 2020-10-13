
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class TaskEngine : MonoBehaviour
{

    public GameObject MainGO;

    private TaskSettings _gameManager;
    private ScannerInHandler _scannerIn;
    private EvalResponse _evalResponse;

    private float _stimDuration = .0333f;
    private float _slowStimDuration = .3330f;
    private float _bckDuration = .9667f;
    private int _fixationTimeOut = 10;
    private int _instructionsTimeOut = 5;
    private float _jitter = 0;
    private bool _taskOver = false;

    public int StimCode;
    public double StimTime;

    public BlockType CurrentBlockType = BlockType.TrialBlock;
    public TaskState CurrentTaskState = TaskState.RunTask;
    public ResponseWindow CurrentResponseState = ResponseWindow.Open;
    public IPerfLog _logger = new FileSystemLogger();

    //arrays to hold sprites
    Sprite[] _spriteArray;
    Sprite[] _waitingSprites;

    //arrays to hold presentation order for the diff blocks
    List<int> _practiceBlock = new List<int>();
    List<int> _trialBlock = new List<int>();
    List<double> _stimTimes = new List<double>();
    List<float> _jitterFull = new List<float>();
    List<float> _interStimInterval = new List<float>();
    
    //only for debug purposes
    List<int> _deBug = new List<int>();

    public void AbortTrial()
    {
        _logger.Term();
    }

    public void ResponseLogger()
    {
        if (StimCode == 0 && _evalResponse.ResponseTime == 0)
        {
            _evalResponse.CurrentResponseEval = ResponseEval.MISS;
            _evalResponse.MissCount++;
        }

        if (StimCode != 0 && _evalResponse.ResponseTime == 0)
        {
            _evalResponse.CurrentResponseEval = ResponseEval.CRJ;
            _evalResponse.CorrectRejectCount++;

        }

        _logger.LogRaw(CurrentBlockType.ToString() + " " + StimCode + " " + StimTime + " " + _evalResponse.ResponseTime + " " + _evalResponse.CurrentResponseEval.ToString() + " " + _evalResponse.HitCount + " " + _evalResponse.FalseAlarmCount + " " + _evalResponse.MissCount + "  " + _evalResponse.CorrectRejectCount);

        _evalResponse.CurrentResponseEval = ResponseEval.Init;
        _evalResponse.RTstamp = 0;
        _evalResponse.ResponseTime = 0;
    }

    public void JitterCalc()
    {
        foreach (float value in _jitterFull)
        {
            float dec = (value / 1000) - _stimDuration;
            _interStimInterval.Add(dec);
        }
    }

    void Awake()
    {
        _scannerIn = FindObjectOfType<ScannerInHandler>();
        _evalResponse = FindObjectOfType<EvalResponse>();
        _gameManager = TaskSettingsManager.TaskSettings;

        //get stim duration from config
        float.TryParse(_gameManager.StimDuration, out _stimDuration);

        //loading sprites into array...
        _spriteArray = Resources.LoadAll<Sprite>("Sprites/Digits");
        _waitingSprites = Resources.LoadAll<Sprite>("Sprites/Others");

        //array of digits to guide order of stimuli presentation...
        _practiceBlock.InsertRange(_practiceBlock.Count, new List<int> { 6, 0, 3, 7, 0, 8, 9, 6, 1, 4, 5, 0, 3, 9, 6, 2, 0, 7, 6, 5, 0, 8, 5, 0, 2, 5, 1, 6, 8, 0, 4, 3, 0, 4, 2, 5, 8, 1, 0, 7, 6, 0, 2, 0, 7, 8, 0, 3, 9, 0, 4, 2, 1, 0, 8, 3, 0, 5, 9, 4, 0, 9, 7, 1, 0, 3, 5, 2, 8, 9, 0, 7, 0, 2, 1, 4, 0, 6, 4, 7, 6, 0, 3, 7, 0, 8, 9, 6, 1, 4, 5, 0, 3, 9, 6, 2, 0, 7, 6, 5, 0, 8, 5, 0, 2, 5, 1, 6, 8, 0, 4, 3, 0, 4, 2, 5, 8, 1, 0, 7, 6, 0, 2, 0, 7, 8, 0, 3, 9, 0, 4, 2, 1, 0, 8, 3, 0, 5, 9, 4, 0, 9, 7, 1, 0, 3, 5, 2, 8, 9, 0, 7, 0, 2, 1, 4, 0, 6, 4, 7 });
        _trialBlock.InsertRange(_trialBlock.Count, new List<int> { 6, 0, 3, 7, 0, 8, 9, 6, 1, 4, 5, 0, 3, 9, 6, 2, 0, 7, 6, 5, 0, 8, 5, 0, 2, 5, 1, 6, 8, 0, 4, 3, 0, 4, 2, 5, 8, 1, 0, 7, 6, 0, 2, 0, 7, 8, 0, 3, 9, 0, 4, 2, 1, 0, 8, 3, 0, 5, 9, 4, 0, 9, 7, 1, 0, 3, 5, 2, 8, 9, 0, 7, 0, 2, 1, 4, 0, 6, 4, 7 });
        _jitterFull.InsertRange(_jitterFull.Count, new List<float> { 1500, 1250, 750, 1250, 1000, 750, 750, 1500, 1000, 750, 1000, 1000, 750, 1500, 750, 750, 1000, 1500, 750, 1250, 1000, 1000, 1250, 750, 1500, 750, 1000, 1000, 750, 750, 1000, 1750, 750, 750, 1250, 750, 750, 750, 750, 1000, 750, 750, 750, 750, 750, 1750, 1250, 750, 750, 750, 1000, 1500, 1000, 1250, 1000, 1250, 1000, 750, 1250, 750, 1250, 750, 1000, 1000, 750, 1750, 750, 1000, 750, 750, 1250, 1000, 750, 1250, 1000, 1500, 750, 1000, 1250, 750 });
       
        ////debug purposes only
        _deBug.InsertRange(_deBug.Count, new List<int> { 6, 0, 3, 7, 0 });
    }

    void Start()
    {
        //load indicated sprites from config...
        if(_gameManager.Border == true)
        {
            _spriteArray = Resources.LoadAll<Sprite>("Sprites/BorderDigits");
        }
        if(_gameManager.Big == true)
        {
            _spriteArray = Resources.LoadAll<Sprite>("Sprites/Digits");
        }

        JitterCalc();

        if (_gameManager.WaitForTrigger == true)
        {
            CurrentTaskState = TaskState.WaitForTrigger;
        }

        StartCoroutine(RunTask());
    }

    //checks if Task done and runs set order of blocks
    IEnumerator RunTask()
    {
        while (CurrentTaskState == TaskState.WaitForTrigger)
        {
            _scannerIn.WaitForTrigger();
            yield return new WaitForSeconds(.005f);
        }

        _logger.Init();

        //condition for ending the game
        while (_taskOver == false)
        {
            if (CurrentTaskState == TaskState.RunTask)
            {
                long start = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);

                yield return StartCoroutine(Fixation());
                yield return StartCoroutine(TrialBlock());
                yield return StartCoroutine(Fixation());
                yield return StartCoroutine(LastTrialBlock());
                yield return StartCoroutine(LastFixation());

                long end = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                long duration = end - start;

                Debug.Log(duration/1000);
            }
        }
        SceneManager.LoadScene("GameEndScreen");
    }
    //method to display fixation cross and instructions
    IEnumerator Fixation()
    {
        bool fixate = true;
        bool instruct = false;
        CurrentBlockType = BlockType.FixationBlock;

        while (fixate == true)
        {
            MainGO.GetComponent<SpriteRenderer>().sprite = _waitingSprites[0];
            MainGO.SetActive(true);

            yield return new WaitForSeconds(_fixationTimeOut);
            MainGO.SetActive(false);

            fixate = false;
            instruct = true;
        }

        while (instruct == true)
        {
            MainGO.GetComponent<SpriteRenderer>().sprite = _waitingSprites[2];
            MainGO.SetActive(true);

            yield return new WaitForSeconds(_instructionsTimeOut);
            MainGO.SetActive(false);

            instruct = false;
        }

    }
    //fixation and instructions for task complete
    IEnumerator LastFixation()
    {
        bool fixate = true;
        bool instruct = false;
        CurrentBlockType = BlockType.FixationBlock;

        while (fixate == true)
        {
            MainGO.GetComponent<SpriteRenderer>().sprite = _waitingSprites[0];
            MainGO.SetActive(true);

            yield return new WaitForSeconds(_fixationTimeOut);
            MainGO.SetActive(false);

            fixate = false;
            instruct = true;
        }

        while (instruct == true)
        {
            MainGO.GetComponent<SpriteRenderer>().sprite = _waitingSprites[3];
            MainGO.SetActive(true);

            yield return new WaitForSeconds(_instructionsTimeOut);
            MainGO.SetActive(false);

            instruct = false;
            //condition for End Task
            _taskOver = true;
        }

    }
    //Trial Block: Actual trial. Presenting 80 stimuli
    IEnumerator TrialBlock()
    {
        Debug.Log("TrialBlock");
        CurrentBlockType = BlockType.TrialBlock;
        MainGO.SetActive(false);

        //reset ISI trackers
        float isi = 0;
        int ISIindex = 0;

        //320 total stimuli presented
        foreach (int value in _trialBlock)
        {

            //storing stim code
            StimCode = value;
            //time stamp when stimulus shown
            StimTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            //storing StimTime in array
            _stimTimes.Add(StimTime);
            //pick out corresponding ISI
            isi = _interStimInterval[ISIindex];

            MainGO.GetComponent<SpriteRenderer>().sprite = _spriteArray[value];
            MainGO.SetActive(true);
            CurrentResponseState = ResponseWindow.Open;

            yield return new WaitForSeconds(_stimDuration);
            MainGO.SetActive(false);

            MainGO.GetComponent<SpriteRenderer>().sprite = _spriteArray[10];
            MainGO.SetActive(true);

            yield return new WaitForSeconds(isi);
            MainGO.SetActive(false);

            //increment ISI to match stimulus
            ISIindex++;
            //log responses
            ResponseLogger();
        }
    }
    //Last trial block...file write happens
    IEnumerator LastTrialBlock()
    {
        Debug.Log("TrialBlock");
        CurrentBlockType = BlockType.TrialBlock;
        MainGO.SetActive(false);

        //reset ISI trackers
        float isi = 0;
        int ISIindex = 0;

        //320 total stimuli presented
        foreach (int value in _trialBlock)
        {

            //storing stim code
            StimCode = value;
            //time stamp when stimulus shown
            StimTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            //storing StimTime in array
            _stimTimes.Add(StimTime);
            //pick out corresponding ISI
            isi = _interStimInterval[ISIindex];

            MainGO.GetComponent<SpriteRenderer>().sprite = _spriteArray[value];
            MainGO.SetActive(true);

            yield return new WaitForSeconds(_stimDuration);
            MainGO.SetActive(false);

            MainGO.GetComponent<SpriteRenderer>().sprite = _spriteArray[10];
            MainGO.SetActive(true);

            yield return new WaitForSeconds(isi);
            MainGO.SetActive(false);

            //increment ISI to match stimulus
            ISIindex++;
            //log responses
            ResponseLogger();
        }
        //write log files
        _logger.Term();
    }

}

public enum BlockType
{
    FixationBlock,
    TrialBlock
}

public enum ResponseWindow
{
    Closed,
    Open
}

public enum TaskState
{
    WaitForTrigger,
    RunTask
}


//order of presentation:
//6, 0, 3, 7, 0, 8, 9, 6, 1, 4, 5, 0, 3, 9, 6, 2, 0, 7, 6, 5, 0, 8, 5, 0, 2, 5, 1, 6, 8, 0, 4, 3, 0, 4, 2, 5, 8, 1, 0, 7, 6, 0, 2, 0, 7, 8, 0, 3, 9, 0, 4, 2, 1, 0, 8, 3, 0, 5, 9, 4, 0, 9, 7, 1, 0, 3, 5, 2, 8, 9, 0, 7, 0, 2, 1, 4, 0, 6, 4, 7 


            //    if (StimFlag1 == 0 && StimFlag2 == 0)
            //{
            //    StimTime1 = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);

            //    StimFlag1 = 1;
            //}

            //else if (StimFlag1 == 1 && StimFlag2 == 0)
            //{
            //    StimTime2 = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);

            //    StimFlag2 = 1;
            //    StimFlag1 = 0;
            //}

            //else if (StimFlag1 == 0 && StimFlag2 == 1)
            //{
            //    StimTime1 = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);

            //    StimFlag2 = 0;
            //    StimFlag1 = 1;
            //}