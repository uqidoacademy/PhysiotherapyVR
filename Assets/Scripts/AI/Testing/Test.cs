﻿using DG.Tweening;
using Limb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public KeyCode keyIdeal = KeyCode.X, keyReal = KeyCode.N;
    public float timing = 1;
    public Vector3 initialPositionShoulder, initialPositionElbow, initialPositionHand;
    GameObject elbow, shoulder, hand;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(keyIdeal))
        {
            Debug.Log("START TEST");

            // Start play ideal

            shoulder = GameObject.FindGameObjectWithTag("shoulder");
            Debug.Log("Shoulder found: " + shoulder != null);

            float tolleranceRadius = 0.2f;
            float timing = 0.5f, totalDuration = 2f;

            // instance sensor
            ArticolationTollerance tollerance = new ArticolationTollerance();
            tollerance.positionSpeedTolleranceRadius = tolleranceRadius;
            tollerance.positionTolleranceRadius = tolleranceRadius;
            tollerance.rotationSpeedTolleranceRadius = tolleranceRadius;
            tollerance.rotationTolleranceRadius = tolleranceRadius;

            Sensor shoulderSensor = new Sensor();
            shoulderSensor.physicalSensor = shoulder;
            shoulderSensor.sensorTollerance = tollerance;

            initialPositionShoulder = shoulder.transform.position;

            elbow = GameObject.FindGameObjectWithTag("elbow");
            Debug.Log("Elbow found: " + elbow != null);

            Sensor elbowSensor = new Sensor();
            elbowSensor.physicalSensor = elbow;
            elbowSensor.sensorTollerance = tollerance;

            initialPositionElbow = elbow.transform.position;

            hand = GameObject.FindGameObjectWithTag("hand");
            Debug.Log("Hand found: " + hand != null);

            Sensor handSensor = new Sensor();
            handSensor.physicalSensor = hand;
            handSensor.sensorTollerance = tollerance;

            initialPositionHand = hand.transform.position;

            LimbConfiguration config = new LimbConfiguration();
            config.limb = LimbsEnum.ARM;
            config.sensors.Add(ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.SHOULDER), shoulderSensor);
            config.sensors.Add(ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.ELBOW), elbowSensor);
            config.sensors.Add(ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.HAND), handSensor);
            config.timing = timing;

            Evaluator eval = Evaluator.Instance;

            eval.configs.Clear();
            eval.configs.Add(config);

            eval.ExerciseSetup(timing);
            eval.StartSetup();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(hand.transform.DOMove(hand.transform.position * 2, totalDuration));
            sequence.OnComplete(() => eval.SaveSetup());

            // Evaluator.Instance.ExerciseSetup(timing);

            // End play ideal

            // Evaluator.Instance.SaveSetup();

            // Start play real

            // Evaluator.Instance.StartEvaluation();

            // End play real

            // Evaluator.Instance.StopEvaluation();



        }
        if(Input.GetKeyDown(keyReal))
        {
            hand.transform.position = initialPositionHand;
            elbow.transform.position = initialPositionElbow;
            shoulder.transform.position = initialPositionShoulder;


            Evaluator.Instance.StartEvaluation();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(hand.transform.DOMove(hand.transform.position * 2, timing / 2));
            sequence.OnComplete(() => Evaluator.Instance.StopEvaluation());
        }
	}
}
