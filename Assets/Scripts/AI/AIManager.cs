using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class EvaluationResults { } // TODO implement this to return results on single step
    public class OverallExerciseResults { } // TODO implement this to return the overall results on the full exercise
    public class OverallSessionResults { } // TODO implement this to return the overall results on the full session

    public interface IAIManager
    {
        // TODO generalize

        #region Arms

        ArmExerciseStep ArmExerciseStepFromSensors(Transform shoulder, Transform elbow, Transform hand, bool isRelative = false);
        int CreateArmSession(ArmExerciseStep[] exerciseSteps, float timing);
        int StartArmExercise(int armSessionID);
        EvaluationResults EvaluateArmStep(ArmExerciseStep exerciseStep, int armExerciseID, int armSessionID);
        OverallSessionResults CloseExerciseSession(int armSessionID);

        #endregion
    }

    public class AIManager : IAIManager
    {
        private List<Core> _armExerciseSessions = new List<Core>();

        public ArmExerciseStep ArmExerciseStepFromSensors(Transform shoulder, Transform elbow, Transform hand, bool isRelative = false)
        {
            return new ArmExerciseStep(shoulder, elbow, hand, isRelative);
        }

        public OverallSessionResults CloseExerciseSession(int armSessionID)
        {
            throw new System.NotImplementedException();
        }

        public int CreateArmSession()
        {
            throw new System.NotImplementedException();
        }

        public int CreateArmSession(ArmExerciseStep[] exerciseSteps, float timing)
        {
            _armExerciseSessions.Add(new Core(exerciseSteps, timing));
            return _armExerciseSessions.Count - 1;
        }

        public EvaluationResults EvaluateArmStep(int armExerciseID, int armSessionID)
        {
            throw new System.NotImplementedException();
        }

        public EvaluationResults EvaluateArmStep(ArmExerciseStep exerciseStep, int armExerciseID, int armSessionID)
        {
            throw new System.NotImplementedException();
        }

        public int StartArmExercise(int armSessionID)
        {
            throw new System.NotImplementedException();
        }
    }
}