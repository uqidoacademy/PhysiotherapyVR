using AI;
using AI.Error;
using AI.Results;
using Limb;
using System;

namespace VRPhysiotheraphyst
{
    public class ExerciseConfiguration
    {
        public LimbConfiguration limbConfiguration;
        public delegate void HandleResults(EvaluationResults results);
        public event HandleResults OnExecutionStepEvaluated;

        public ExerciseConfiguration(LimbConfiguration limbConfig, HandleResults resultsHandler)
        {
            limbConfiguration = limbConfig;
            OnExecutionStepEvaluated += resultsHandler;
        }

        public void ProvideExerciseResults(EvaluationResults results)
        {
            OnExecutionStepEvaluated(results);
        }
    }
}