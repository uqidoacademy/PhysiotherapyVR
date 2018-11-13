using AI;
using AI.Error;
using AI.Proxy;
using AI.Results;
using DG.Tweening;
using Limb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRPhysiotheraphyst;

public class SenderExerciseAI : MonoBehaviour {
    
    public float timing = 1;
    public GameObject elbow, shoulder, hand;

    public ExerciseConfiguration exerciseConfiguration;

    public bool isThisSample = false;

    public void StartSendRecording()
    {
        Debug.Log("start recording");

    if(isThisSample == false)
        {
            CreateAI();
            VirtualPhysioterphyst.Instance.StartSetup();
        }
        else
        {
            VirtualPhysioterphyst.Instance.StartEvaluation();
        }
    }

    public void StopSendRecording()
    {
        Debug.Log("stop recording");

        if (isThisSample == false)
        {
            VirtualPhysioterphyst.Instance.SaveSetup();
        }
        else
        {
            VirtualPhysioterphyst.Instance.StopEvaluation();
        }
    }


    private void CreateAI()
    {
        float tolleranceRadius = 0.2f;
        ArticolationTollerance tollerance = new ArticolationTollerance();
        tollerance.positionSpeedTolleranceRadius = tolleranceRadius;
        tollerance.positionTolleranceRadius = tolleranceRadius;
        tollerance.rotationSpeedTolleranceRadius = tolleranceRadius;
        tollerance.rotationTolleranceRadius = tolleranceRadius;
        Sensor shoulderSensor = new Sensor(shoulder, tollerance, "spalla");
        Sensor elbowSensor = new Sensor(elbow, tollerance, "gomito");
        Sensor handSensor = new Sensor(hand, tollerance, "mano");

        LimbConfiguration config = new LimbConfiguration(shoulderSensor, elbowSensor, handSensor);
        exerciseConfiguration = new ExerciseConfiguration(
            config,
            (EvaluationResults results) =>
            {
                AIProxy aiProxy = new ArmAIProxy(); // should be taken from context
                ArticolationError elbowError = aiProxy.UnwrapFromResults("gomito", results);
            }
            );
        exerciseConfiguration.OnExecutionStepEvaluated += (EvaluationResults results) => { }; // exercise configuration should be taken from somewhere
        VirtualPhysioterphyst eval = VirtualPhysioterphyst.Instance;
        float timing = 0.5f, totalDuration = 2f;
        eval.timingBetweenSamples = timing;
        eval.ExerciseSetup(exerciseConfiguration);
    }

    #region DemoUtils 

    #endregion
}
