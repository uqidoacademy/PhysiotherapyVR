﻿using AI;
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


    public void StartSendRecording()
    {
    Debug.Log("start recording");
        UIDesktopManager.I.RegistrationFeedback(true);
    if(isThisExercise == false)
        {
            CreateAI();
            VirtualPhysioterphyst.Instance.StartSetup();
            sampleRecorder.StartRecording();
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
        sampleRecorder = GetComponent<SampleRecorder>();
        Sensor shoulderSensor = new Sensor(shoulder, shoulderTollerance, "spalla");
        Sensor elbowSensor = new Sensor(elbow, elbowTollerance, "gomito");
        Sensor handSensor = new Sensor(hand, handTollerance, "mano");

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
            }
            );
        exerciseConfiguration.OnExecutionStepEvaluated += (EvaluationResults results) => { }; // exercise configuration should be taken from somewhere
        VirtualPhysioterphyst eval = VirtualPhysioterphyst.Instance;
        float timing = 0.5f, totalDuration = 2f;
        eval.timingBetweenSamples = timing;
        eval.ExerciseSetup(exerciseConfiguration);
    }

}
