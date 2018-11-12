using System.Collections.Generic;
using AI;
using Limb;
using UnityEngine;

public class VirtualPhysioterapy : MonoBehaviour
{
    // TODO (v2.0) parallelize all this stuf (should be easy but require some time)

    public List<LimbConfiguration> configs = new List<LimbConfiguration>();
    private AIManager mAIManager;
    private float timing = 0.1f;

    #region Singleton

    private static VirtualPhysioterapy _instance = null;
    public static VirtualPhysioterapy Instance
    {
        get
        {
            if (_instance == null) throw new MissingReferenceException("Sampler instance accessed before initialization!");
            return _instance;
        }
    }

    private void SetupSingleton()
    {
        if (_instance == null)
        {
            _instance = this;
            // DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }

    #region AI API usage
    
    #region Sampling activities

    // TODO handle different exercises
    private List<ExerciseStep> _idealStepsSampling = new List<ExerciseStep>();

    private void HandlerOnSetup(Sample sample)
    {
        foreach (LimbData limbData in sample.limbSamples)
        {
            _idealStepsSampling.Add(limbData.UnwrapFromSensors());
            return; // TODO handle different exercises (only one allowed at the moment)
        }
    }

    private void HandlerOnExecution(Sample sample)
    {
        foreach (LimbData limbData in sample.limbSamples)
        {
            EvaluationResults results = mAIManager.EvaluateExerciseStep((ArmExerciseStep) limbData.UnwrapFromSensors());
            // TODO implement error handling
            Debug.Log(results.NiceWork);
            return; // TODO handle different exercises (only one allowed at the moment)
        }
    }
    #endregion

    public void ExerciseSetup(float timing)
    {
        this.timing = timing;
        mAIManager = new AIManager();
        // TODO: fix AI Manager and use all the potential of this class (not only one limb and one exercise)
        LimbConfiguration config = configs[0];

        Dictionary<string, ArticolationTollerance> tollerance = new Dictionary<string, ArticolationTollerance>();
        foreach (string sensorArticolationName in config.sensors.Keys)
        {
            tollerance.Add(sensorArticolationName, config.sensors[sensorArticolationName].sensorTollerance);
        }
        mAIManager.ExerciseTollerance = tollerance;
    }

    HandleSample setupSampleHandler = null;

    public void StartSetup()
    {
        // TODO: fix AI Manager and use all the potential of this class
        LimbConfiguration config = configs[0];
        setupSampleHandler = HandlerOnSetup;

        OnSampleTaken += setupSampleHandler;
        StartSampling(timing);
    }

    public void StopSetup(bool save = true)
    {
        StopSampling();
        OnSampleTaken -= setupSampleHandler;

        // TODO handle more exercises
        if (save) mAIManager.CreateExerciseSession(_idealStepsSampling.ToArray(), timing);

        _idealStepsSampling.Clear();
    }

    public void SaveSetup() { StopSetup(true); }

    public void DiscardSetup() { StopSetup(false); }

    HandleSample executionSampleHandler = null;

    public void StartEvaluation()
    {
        // TODO: fix AI Manager and use all the potential of this class
        LimbConfiguration config = configs[0];
        executionSampleHandler = HandlerOnExecution;

        OnSampleTaken += executionSampleHandler;
        StartSampling(timing);
    }

    public void StopEvaluation()
    {
        StopSampling();
        OnSampleTaken -= executionSampleHandler;
    }

    #endregion

    #region Sampling Utils

    public class Sample
    {
        public List<LimbData> limbSamples = new List<LimbData>();
    }

    delegate void HandleSample(Sample sample);
    event HandleSample OnSampleTaken;

    private void StartSampling(float timing)
    {
        InvokeRepeating("SampleSensors", 0.0f, timing);
    }

    private void StopSampling()
    {
        CancelInvoke();
    }

    private void SampleSensors()
    {
        Sample sample = new Sample();
        for (int i = 0; i < configs.Count; i++)
        {
            LimbConfiguration config = configs[i];
            sample.limbSamples.Add(config.ExtractLimbData());
        }
        OnSampleTaken(sample);
    }

    #endregion

    #endregion

    private void Awake()
    {
        SetupSingleton();
    }
}

