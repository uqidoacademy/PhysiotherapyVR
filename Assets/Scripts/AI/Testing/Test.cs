using AI;
using DG.Tweening;
using Limb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example usage of VirtualPhysioteraphyst
/// </summary>
/// <author>Antonio Terpin</author>
public class Test : MonoBehaviour {
    
    public float timing = 1;
    GameObject elbow, shoulder, hand;

    // Implement a results handler
    public class ResultsHandler : VirtualPhysioterphyst.IResultsHandler
    {
        public void HandleResults(EvaluationResults results)
        {
            Debug.Log(results.NiceWork);
        }
    } 

    void Update () {
		if(Input.GetKeyDown(keyIdeal))
        {
            // settings

            // define tollerance (in this case is used the same tollerance for all variables and articulations
            float tolleranceRadius = 0.2f;
            ArticolationTollerance tollerance = new ArticolationTollerance();
            tollerance.positionSpeedTolleranceRadius = tolleranceRadius;
            tollerance.positionTolleranceRadius = tolleranceRadius;
            tollerance.rotationSpeedTolleranceRadius = tolleranceRadius;
            tollerance.rotationTolleranceRadius = tolleranceRadius;

            // Configure limb, one per exercise per limb (if you want an exercise that involves to limbs, use two limbs configurations)

            // example made for the arm

            // instance sensors
            shoulder = GameObject.FindGameObjectWithTag("shoulder");
            Sensor shoulderSensor = new Sensor();
            shoulderSensor.physicalSensor = shoulder;
            shoulderSensor.sensorTollerance = tollerance;
            
            elbow = GameObject.FindGameObjectWithTag("elbow");
            Sensor elbowSensor = new Sensor();
            elbowSensor.physicalSensor = elbow;
            elbowSensor.sensorTollerance = tollerance;

            hand = GameObject.FindGameObjectWithTag("hand");
            Sensor handSensor = new Sensor();
            handSensor.physicalSensor = hand;
            handSensor.sensorTollerance = tollerance;

            // with the sensors instances create a limb configuration

            LimbConfiguration config = new ArmConfiguration(shoulderSensor, elbowSensor, handSensor);

            // configure the virtual physioteraphyst
            VirtualPhysioterphyst eval = VirtualPhysioterphyst.Instance;

            // define a timing for the sampling
            float timing = 0.5f, totalDuration = 2f;
            eval.timingBetweenSamples = timing;

            // assign the results handler defined above
            eval.resultsHandler = new ResultsHandler();

            // setup the exercise with the built configuration
            eval.ExerciseSetup(config);

            // start recording
            eval.StartSetup();

            // Start playing a new exercise (guided by the real physioteraphyst)
            Sequence sequence = DOTween.Sequence();
            sequence.Append(hand.transform.DOMove(hand.transform.position * 2, totalDuration));

            // once finished the exercise, stop setup
            sequence.OnComplete(() => eval.SaveSetup());

        }
        if(Input.GetKeyDown(keyReal))
        {
            // turn back to initial position
            hand.transform.position = initialPositionHand;
            elbow.transform.position = initialPositionElbow;
            shoulder.transform.position = initialPositionShoulder;

            // start evaluation of the exercise
            VirtualPhysioterphyst.Instance.StartEvaluation();

            // play the exercise
            Sequence sequence = DOTween.Sequence();
            sequence.Append(hand.transform.DOMove(hand.transform.position * 2, timing / 2));

            // on finish stop evaluating
            sequence.OnComplete(() => VirtualPhysioterphyst.Instance.StopEvaluation());
        }
	}

    #region DemoUtils 

    public KeyCode keyIdeal = KeyCode.X, keyReal = KeyCode.N;
    public Vector3 initialPositionShoulder, initialPositionElbow, initialPositionHand;

    private void Awake()
    {
        initialPositionShoulder = shoulder.transform.position;
        initialPositionElbow = elbow.transform.position;
        initialPositionHand = hand.transform.position;
    }

    #endregion
}
