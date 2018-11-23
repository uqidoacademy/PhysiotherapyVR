using System.Collections.Generic;
using AI;
using AI.Results;
using Limb;
using UnityEngine;

namespace VRPhysiotheraphyst
{

    /// <summary>
    /// Class used to register and evaluate errors made from the patient
    /// </summary>
    /// TODO: use session concept, to provide repetitions and so on and so forth
    /// <author>
    /// Antonio Terpin
    /// </author>
    public class VirtualPhysioterphyst : MonoBehaviour
    {
        // TODO (v2.0) parallelize all this stuf (should be easy but require some time

        #region Singleton
        public float timingBetweenSamples = 0.5f;

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
            public ExerciseConfiguration exerciseConfig;
            public List<ExerciseStep> idealStepsSampling = new List<ExerciseStep>();
            public List<ExerciseStep> realStepsSampling = new List<ExerciseStep>();
            public AIManager aiManager = new AIManager();
            public bool isTemporary = true;
        }

        private List<LimbExercise> _exercises = new List<LimbExercise>();
        public List<ExerciseStep> GetIdealSamplingForExercise(int index)
        {
            return _exercises[index].idealStepsSampling;
        }

        public List<ExerciseStep> GetRealSamplingForExercise(int index)
        {
            return _exercises[index].realStepsSampling;
        }

        public int GetExerciseCount()
        {
            return _exercises.Count;
        }

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
                LimbExercise ex = _exercises[exID];
                EvaluationResults results = (ex.exerciseConfig.isRealTimeSampling) ?
                    ex.aiManager.EvaluateExerciseStep(sample.sampleDataForExercise[exID].UnwrapFromSensors(), sample.idealDataForExercise[exID].UnwrapFromSensors()) :
                    ex.aiManager.EvaluateExerciseStep(sample.sampleDataForExercise[exID].UnwrapFromSensors());
                ex.exerciseConfig.ProvideExerciseResults(results);
            };
        }

        #endregion

        #region Exercise setup and ideal movements recording

        /// <summary>
        /// Used to setup a new limb exercise
        /// </summary>
        /// <param name="config">Configuration of the limb that has to perform the exercise</param>
        public void ExerciseSetup(ExerciseConfiguration config)
        {
            LimbExercise exercise = new LimbExercise();
            exercise.exerciseConfig = config;

            ArticolationTollerance[] tollerance = new ArticolationTollerance[config.limbConfiguration.sensors.Length];
            for(int i = 0; i < config.limbConfiguration.sensors.Length; i++)
            {
                tollerance[i] = config.limbConfiguration.sensors[i].sensorTollerance;
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
            for (int exID = 0; exID < _exercises.Count; exID++)
            {
                LimbExercise exercise = _exercises[exID];
                if(!exercise.exerciseConfig.isRealTimeSampling)
                {
                    // need setup
                    setupSampleHandlers[exID] = GetIdealSamplerHandlerForExercise(exID);
                    OnSampleTaken += setupSampleHandlers[exID];
                } else
                {
                    // no need of setup
                    exercise.isTemporary = false;
                    exercise.aiManager.CreateExerciseSession(timingBetweenSamples); // creating a session without tranining set
                }
            }

            StartSampling(timingBetweenSamples);
        }

        private void StopSetup(bool save = true)
        {
            foreach (LimbExercise ex in _exercises)
            {
                if (ex.isTemporary)
                {
                    if (save)
                    {
                        ex.aiManager.CreateExerciseSession(timingBetweenSamples, ex.idealStepsSampling.ToArray());
                        ex.isTemporary = false;
                    }
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

        /// <summary>
        /// Use this to end registering the setup (not saving or discarding it yet)
        /// </summary>
        public void EndSetup()
        {
            StopSampling();
            foreach (HandleSample handler in setupSampleHandlers) OnSampleTaken -= handler;
            setupSampleHandlers = null;
        }



        #endregion

        #region Execution sampling

        HandleSample[] executionSampleHandlers = null;

        /// <summary>
        /// Use this to start evaluating the recorded exercises
        /// </summary>
        public void StartEvaluation()
        {
            executionSampleHandlers = new HandleSample[_exercises.Count];
            for (int exID = 0; exID < _exercises.Count; exID++)
            {
                LimbExercise exercise = _exercises[exID];
                exercise.realStepsSampling.Clear();
                if(exercise.exerciseConfig.isRealTimeSampling)
                {
                    exercise.idealStepsSampling.Clear();
                }
                exercise.aiManager.StartExercise(exercise.exerciseConfig.isRealTimeSampling);
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
            public LimbData[] idealDataForExercise;
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
            sample.idealDataForExercise = new LimbData[_exercises.Count];
            for (int exID = 0; exID < _exercises.Count; exID++)
            {
                LimbExercise exercise = _exercises[exID];
                sample.sampleDataForExercise[exID] = exercise.exerciseConfig.limbConfiguration.ExtractLimbData();
                if (exercise.exerciseConfig.isRealTimeSampling)
                {
                    sample.idealDataForExercise[exID] = exercise.exerciseConfig.ghostLimbConfiguration.ExtractLimbData();
                }
            }
            if(OnSampleTaken != null) OnSampleTaken(sample);
        }

        #endregion

        #endregion

        private void Awake()
        {
            SetupSingleton();
        }
    }

}
