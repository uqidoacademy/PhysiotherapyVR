using System.Collections.Generic;
using AI;
using Limb;
using UnityEngine;

public class VirtualPhysioterphyst : MonoBehaviour
{
    // TODO (v2.0) parallelize all this stuf (should be easy but require some time)

    public interface IResultsHandler {
        void HandleResults(EvaluationResults results);
    }

    #region Singleton
    public float timingBetweenSamples = 0.5f;
    public IResultsHandler resultsHandler = null;

    private static VirtualPhysioterphyst _instance = null;
    public static VirtualPhysioterphyst Instance
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
    private class LimbExercise
    {
        public LimbConfiguration exerciseConfig;
        public List<ExerciseStep> idealStepsSampling = new List<ExerciseStep>();
        public List<ExerciseStep> realStepsSampling = new List<ExerciseStep>();
        public AIManager aiManager = new AIManager();
        public bool isTemporary = true;
    }

    private List<LimbExercise> _exercises = new List<LimbExercise>();

    #region Sampling activities

    private HandleSample GetIdealSamplerHandlerForExercise(int exID)
    {
        return (Sample sample) =>
        {
            _exercises[exID].idealStepsSampling.Add(sample.sampleDataForExercise[exID].UnwrapFromSensors());
        };
    }

    private HandleSample GetExecutionSamplerHandlerForExercise(int exID)
    {
        return (Sample sample) =>
        {
            EvaluationResults results = _exercises[exID].aiManager.EvaluateExerciseStep(sample.sampleDataForExercise[exID].UnwrapFromSensors());
            if (resultsHandler != null) resultsHandler.HandleResults(results);
        };
    }
    
    #endregion

    #region Exercise setup and ideal movements recording

    /// <summary>
    /// Used to setup a new limb exercise
    /// </summary>
    /// <param name="config">Configuration of the limb that has to perform the exercise</param>
    public void ExerciseSetup(LimbConfiguration config)
    {
        LimbExercise exercise = new LimbExercise();
        exercise.exerciseConfig = config;

        Dictionary<string, ArticolationTollerance> tollerance = new Dictionary<string, ArticolationTollerance>();
        foreach (string sensorArticolationName in config.sensors.Keys)
        {
            tollerance.Add(sensorArticolationName, config.sensors[sensorArticolationName].sensorTollerance);
        }
        exercise.aiManager.ExerciseTollerance = tollerance;

        _exercises.Add(exercise);
    }

    HandleSample[] setupSampleHandlers;

    /// <summary>
    /// Start recording exercise steps for the set up exercise (ExerciseSetup method)
    /// </summary>
    public void StartSetup()
    {
        setupSampleHandlers = new HandleSample[_exercises.Count];
        for(int exID = 0; exID < _exercises.Count; exID++)
        {
            setupSampleHandlers[exID] = GetIdealSamplerHandlerForExercise(exID);
            OnSampleTaken += setupSampleHandlers[exID];
        }
        
        StartSampling(timingBetweenSamples);
    }

    private void StopSetup(bool save = true)
    {
        StopSampling();
        foreach(HandleSample handler in setupSampleHandlers) OnSampleTaken -= handler;
        setupSampleHandlers = null;

        foreach (LimbExercise ex in _exercises)
        {
            if(ex.isTemporary)
            {
                if (save) ex.aiManager.CreateExerciseSession(ex.idealStepsSampling.ToArray(), timingBetweenSamples);
                else _exercises.Remove(ex);
            }
        }
    }

    /// <summary>
    /// Use this to save the registered setup
    /// </summary>
    public void SaveSetup() { StopSetup(true); }

    /// <summary>
    /// Use this to discard the registered setup
    /// </summary>
    public void DiscardSetup() { StopSetup(false); }

    #endregion

    #region Execution sampling

    HandleSample[] executionSampleHandlers = null;

    /// <summary>
    /// Use this to start evaluating the recorded exercises
    /// </summary>
    public void StartEvaluation()
    {
        executionSampleHandlers = new HandleSample[_exercises.Count];
        for(int exID = 0; exID < _exercises.Count; exID++)
        {
            executionSampleHandlers[exID] = GetExecutionSamplerHandlerForExercise(exID);
            OnSampleTaken += executionSampleHandlers[exID];
        }
        StartSampling(timingBetweenSamples);
    }

    /// <summary>
    /// Use this to stop evaluated the recorded exercises
    /// </summary>
    public void StopEvaluation()
    {
        StopSampling();
        foreach (HandleSample handler in executionSampleHandlers) OnSampleTaken -= handler;
        executionSampleHandlers = null;
    }

    #endregion

    #endregion

    #region Sampling Utils

    public class Sample
    {
        public LimbData[] sampleDataForExercise;
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
        sample.sampleDataForExercise = new LimbData[_exercises.Count];
        for(int exID = 0; exID < _exercises.Count; exID++)
        {
            sample.sampleDataForExercise[exID] = _exercises[exID].exerciseConfig.ExtractLimbData();
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

