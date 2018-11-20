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

    public ArticolationTollerance shoulderTollerance;
    public ArticolationTollerance elbowTollerance;
    public ArticolationTollerance handTollerance;
    
    public delegate void SendResultOfAI(string limb,bool correctPosition);
    public static SendResultOfAI EventSendResultAI;

    public float timing = 1;
    public GameObject elbow, shoulder, hand;

    public ExerciseConfiguration exerciseConfiguration;
    
    public bool isThisExercise = false;

    private SampleRecorder sampleRecorder;

    private bool _initialized = false;
    private void _Init()
    {
        sampleRecorder = GetComponent<SampleRecorder>();
        CreateAI();
        _initialized = true;
    }

    public void StartSendRecording()
    {
        if (!_initialized) _Init();

    Debug.Log("start recording");
        UIDesktopManager.I.RegistrationFeedback(true);
    if(isThisExercise == false)
        {
            CreateAI();
            sampleRecorder.StartRecording();
            VirtualPhysioterphyst.Instance.StartSetup();
        }
        else
        {
            sampleRecorder.StartPlayback();
            VirtualPhysioterphyst.Instance.StartEvaluation();
        }
    }

    public void StopSendRecording()
    {
        Debug.Log("stop recording");
        UIDesktopManager.I.RegistrationFeedback(false);
        if (isThisExercise == false)
        {
            sampleRecorder.StopRecording();
            VirtualPhysioterphyst.Instance.EndSetup();

            // todo do this on go to exercise click, else if repeat use .DiscardSetup()
            VirtualPhysioterphyst.Instance.SaveSetup();
        }
        else
        {
            sampleRecorder.StopPlayback();
            VirtualPhysioterphyst.Instance.StopEvaluation();
        }
    }

    public void SendResult(string limb,bool correctPosition) {
        if (EventSendResultAI != null)
            EventSendResultAI(limb,correctPosition);
    }

    private void CreateAI()
    {
        Sensor shoulderSensor = new Sensor(shoulder, shoulderTollerance, "spalla");
        Sensor elbowSensor = new Sensor(elbow, elbowTollerance, "gomito");
        Sensor handSensor = new Sensor(hand, handTollerance, "mano");

        LimbConfiguration config = new LimbConfiguration(shoulderSensor, elbowSensor, handSensor);

        // check that there is the ghost
        if (sampleRecorder.trackersPreview.Count != 3) throw new System.Exception("Ghost non correttamente configurato");

        LimbConfiguration ghostConfig = new LimbConfiguration(new Sensor(sampleRecorder.trackersPreview[0]), new Sensor(sampleRecorder.trackersPreview[1]), new Sensor(sampleRecorder.trackersPreview[2]));

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

                    GameObject trackerOb;
                    string nameID = "";

                    switch (articolationName)
                    {
                        case "spalla":
                            nameID = "Shoulder";
                            trackerOb = shoulder;
                            break;
                        case "gomito":
                            nameID = "Elbow";
                            trackerOb = elbow;
                            break;
                        case "mano":
                            nameID = "Wrist";
                            trackerOb = hand;
                            break;
                        default:
                            trackerOb = hand;
                            break;
                    }

                    //Genera evento con risultati AI -> contesto
                    SendResult(nameID, isPositionCorrect);


                    if (isPositionCorrect)
                        trackerOb.GetComponent<MeshRenderer>().material.color = Color.green;
                    else
                        trackerOb.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            },
            ghostConfig
            );
        VirtualPhysioterphyst.Instance.ExerciseSetup(exerciseConfiguration);
    }

}
