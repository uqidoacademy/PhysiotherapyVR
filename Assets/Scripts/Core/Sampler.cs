using System.Collections.Generic;
using AI;
using Limb;
using UnityEngine;

namespace Evaluator
{
    /// <summary>
    /// Use this class to sample sensors data.
    /// </summary>
    public class Evaluator : MonoBehaviour
    {
        // TODO (v2.0) parallelize all this stuf (should be easy but require some time)
        // Email me at antonio.terpin@gmai.com to get more info about all this structure

        [SerializeField] public List<LimbConfiguration> configs;
        private AIManager mAIManager;
        private float timing = 0.1f;

        #region Singleton

        private static Evaluator _instance = null;
        public static Evaluator Instance
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
                DontDestroyOnLoad(this);
            }
            else Destroy(this);
        }

        #region AI API usage

        // TODO generalize this (v2.0)
        #region Arms
        private List<ArmExerciseStep> idealArmsStepsSampling = new List<ArmExerciseStep>();
        private ArmExerciseStep UnwrapArmFromSensors(Dictionary<string, Transform> sensorsData)
        {
            return mAIManager.ArmExerciseStepFromSensors(
                sensorsData[ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.SHOULDER)],
                sensorsData[ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.ELBOW)],
                sensorsData[ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.HAND)]
                );
        }

        private void ArmHandlerOnSetup(Sample armSample)
        {
            foreach(LimbData limbData in armSample.limbSamples)
            {
                // TODO generalize
                if(limbData.limb == LimbsEnum.ARM)
                {
                    idealArmsStepsSampling.Add(UnwrapArmFromSensors(limbData.articolations));
                    return;
                }
            }
        }

        private void ArmHandlerOnExecution(Sample armSample)
        {
            foreach (LimbData limbData in armSample.limbSamples)
            {
                // TODO generalize
                if (limbData.limb == LimbsEnum.ARM)
                {
                    EvaluationResults results = mAIManager.EvaluateArmStep(UnwrapArmFromSensors(limbData.articolations));
                    Debug.Log(results.NiceWork);
                    return;
                }
            }
        }
        #endregion

        public void ExerciseSetup(float timing)
        {
            this.timing = timing;
            mAIManager = new AIManager();
            // TODO: fix AI Manager and use all the potential of this class
            LimbConfiguration config = configs[0];
            if (config.limb != LimbsEnum.ARM) throw new System.Exception("Not handled");
            Dictionary<string, ArticolationTollerance> tollerance = new Dictionary<string, ArticolationTollerance>();
            foreach (string sensorArticolationName in config.sensors.Keys)
            {
                tollerance.Add(sensorArticolationName, config.sensors[sensorArticolationName].sensorTollerance);
            }
            switch (config.limb)
            {
                case LimbsEnum.ARM:
                    {
                        mAIManager.ArmTollerance = tollerance;
                    }
                    break;
            }
        }

        public void StartSetup()
        {
            HandleSample sampleHandler = null;
            // TODO: fix AI Manager and use all the potential of this class
            LimbConfiguration config = configs[0];
            if (config.limb != LimbsEnum.ARM) throw new System.Exception("Not handled");

            switch(config.limb)
            {
                case LimbsEnum.ARM:
                    {
                        sampleHandler = ArmHandlerOnSetup;
                    } break;
            }

            OnSampleTaken += sampleHandler;
        }

        public void StopSetup(bool save = true)
        {
            StopSampling();
            mAIManager.CreateArmSession(idealArmsStepsSampling.ToArray(), timing);
            idealArmsStepsSampling.Clear();
        }

        public void SaveSetup() { StopSetup(true); }

        public void DiscardSetup() { StopSetup(false); }

        public void StartEvaluation()
        {
            HandleSample sampleHandler = null;
            // TODO: fix AI Manager and use all the potential of this class
            LimbConfiguration config = configs[0];
            if (config.limb != LimbsEnum.ARM) throw new System.Exception("Not handled");

            switch (config.limb)
            {
                case LimbsEnum.ARM:
                    {
                        sampleHandler = ArmHandlerOnExecution;
                    }
                    break;
            }

            OnSampleTaken += sampleHandler;
        }

        public void StopEvaluation()
        {
            StopSampling();
        }

        #endregion

        #region Sampling Utils

        public class Sample
        {
            public List<LimbData> limbSamples;
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
                LimbData limbData = new LimbData();
                limbData.limb = config.limb;
                foreach (string sensorArticolationName in config.sensors.Keys)
                {
                    limbData.articolations.Add(sensorArticolationName, config.sensors[sensorArticolationName].physicalSensor.transform);
                }
                sample.limbSamples.Add(limbData);
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
}

