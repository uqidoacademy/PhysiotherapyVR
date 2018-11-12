using AI;
using DG.Tweening;
using Limb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public KeyCode keyIdeal = KeyCode.X, keyReal = KeyCode.N;
    public float timing = 1;
    public Vector3 initialPositionShoulder, initialPositionElbow, initialPositionHand;
    GameObject elbow, shoulder, hand;

    public class ResultsHandler : VirtualPhysioterphyst.IResultsHandler
    {
        public void HandleResults(EvaluationResults results)
        {
            Debug.Log(results.NiceWork);
        }
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

            LimbConfiguration config = new ArmConfiguration(shoulderSensor, elbowSensor, handSensor);

            VirtualPhysioterphyst eval = VirtualPhysioterphyst.Instance;

            eval.timingBetweenSamples = timing;
            eval.resultsHandler = new ResultsHandler();

            eval.ExerciseSetup(config);
            eval.StartSetup();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(hand.transform.DOMove(hand.transform.position * 2, totalDuration));
            sequence.OnComplete(() => eval.SaveSetup());

        }
        if(Input.GetKeyDown(keyReal))
        {
            hand.transform.position = initialPositionHand;
            elbow.transform.position = initialPositionElbow;
            shoulder.transform.position = initialPositionShoulder;


            VirtualPhysioterphyst.Instance.StartEvaluation();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(hand.transform.DOMove(hand.transform.position * 2, timing / 2));
            sequence.OnComplete(() => VirtualPhysioterphyst.Instance.StopEvaluation());
        }
	}
}
