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
    
    public delegate void SendResultOfAI(string limb,bool correctPosition);
    public static SendResultOfAI EventSendResultAI;

    public float timing = 1;
    public GameObject elbow, shoulder, hand;

    public ExerciseConfiguration exerciseConfiguration;

    public bool isThisExercise = false;

    public void StartSendRecording()
    {
    Debug.Log("start recording");

    if(isThisExercise == false)
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

        if (isThisExercise == false)
        {
            VirtualPhysioterphyst.Instance.SaveSetup();
        }
        else
        {
            VirtualPhysioterphyst.Instance.StopEvaluation();
        }
    }

    public void SendResult(string limb,bool correctPosition) {
        EventSendResultAI(limb,correctPosition);
    }

    private void CreateAI()
    {
        float tolleranceRadius = 0.1f;
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
                AIProxy aiProxy = new AIProxy(); // should be taken from context
                List<string> articolationNames = new List<string>() { "spalla", "gomito", "mano" };
                foreach(string articolationName in articolationNames)
                {
                    ArticolationError error = aiProxy.UnwrapFromResults(articolationName, results, articolationNames);
                    bool isPositionCorrect = error.Position.IsSpeedCorrect && error.Position.IsMagnitudeCorrect,
                    isRotationCorrect = error.Angle.IsSpeedCorrect && error.Angle.IsMagnitudeCorrect;
                    Debug.Log(articolationName + ": POSITION IS CORRECT - " + isPositionCorrect.ToString() 
                        + " # ROTATION IS CORRECT - " + isRotationCorrect.ToString());

                    //Genera evento con risultati AI -> contesto
                    SendResult(articolationName,isPositionCorrect);

                    GameObject trackerOb;

                    switch (articolationName)
                    {
                        case "spalla":
                            trackerOb = shoulder;
                            break;
                        case "gomito":
                            trackerOb = elbow;
                            break;
                        case "mano":
                            trackerOb = hand;
                            break;
                        default:
                            trackerOb = hand;
                            break;
                    }

                    if (isPositionCorrect)
                        trackerOb.GetComponent<MeshRenderer>().material.color = Color.green;
                    else
                        trackerOb.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
            );
        exerciseConfiguration.OnExecutionStepEvaluated += (EvaluationResults results) => { }; // exercise configuration should be taken from somewhere
        VirtualPhysioterphyst eval = VirtualPhysioterphyst.Instance;
        float timing = 0.5f, totalDuration = 2f;
        eval.timingBetweenSamples = timing;
        eval.ExerciseSetup(exerciseConfiguration);
    }

}
