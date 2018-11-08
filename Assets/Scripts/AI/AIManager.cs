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
        // TODO generalize (v2.0)

        #region Arms

        ArmExerciseStep ArmExerciseStepFromSensors(Transform shoulder, Transform elbow, Transform hand);
        int CreateArmSession(ArmExerciseStep[] exerciseSteps, float timing);
        int StartArmExercise(int armSessionID);
        EvaluationResults EvaluateArmStep(ArmExerciseStep exerciseStep, int armExerciseID, int armSessionID);
        OverallSessionResults CloseExerciseSession(int armSessionID);

        #endregion
    }


    public class AIManager : IAIManager
    {
        private List<Core> _armExerciseSessions = new List<Core>();

        public ArmExerciseStep ArmExerciseStepFromSensors(Transform shoulder, Transform elbow, Transform hand)
        {
            elbow = GlobalTransformToRelative(shoulder, elbow);
            hand = GlobalTransformToRelative(elbow, hand);
            return new ArmExerciseStep(shoulder, elbow, hand);
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

        #region Convertions

        private GameObject _utilityTranformGMOne = null;
        private GameObject UtilityTranformGMOne
        {
            get
            {
                if (_utilityTranformGMOne == null)
                {
                    _utilityTranformGMOne = new GameObject();
                    _utilityTranformGMOne.SetActive(false);
                }
                return _utilityTranformGMOne;
            }
        }

        private GameObject _utilityTranformGMTwo = null;
        private GameObject UtilityTranformGMTwo
        {
            get
            {
                if (_utilityTranformGMTwo == null)
                {
                    _utilityTranformGMTwo = new GameObject();
                    _utilityTranformGMTwo.SetActive(false);
                }
                return _utilityTranformGMTwo;
            }
        }

        public Transform GlobalTransformToRelative(Transform father, Transform child)
        {
            UtilityTranformGMOne.transform.position = father.position;
            UtilityTranformGMOne.transform.eulerAngles = father.eulerAngles;
            UtilityTranformGMTwo.transform.position = child.position;
            UtilityTranformGMTwo.transform.eulerAngles = child.eulerAngles;

            UtilityTranformGMTwo.transform.SetParent(UtilityTranformGMOne.transform);

            // TODO casomai occhio ai puntatori del ciesso
            return UtilityTranformGMTwo.transform;
        }

        #endregion
    }
}