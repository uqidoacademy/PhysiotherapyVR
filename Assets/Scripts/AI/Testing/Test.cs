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

/// <summary>
/// Example usage of VirtualPhysioteraphyst
/// </summary>
/// <author>Antonio Terpin</author>
public class Test : MonoBehaviour {
    
    public float timing = 1;
    public GameObject elbow, shoulder, hand;

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
            Sensor shoulderSensor = new Sensor(shoulder, tollerance, "spalla");
            
            Sensor elbowSensor = new Sensor(elbow, tollerance, "gomito");

            Sensor handSensor = new Sensor(hand, tollerance, "mano");

            // with the sensors instances create a limb configuration

            LimbConfiguration config = new LimbConfiguration(shoulderSensor, elbowSensor, handSensor);

            // configure exercise

            ExerciseConfiguration exerciseConfiguration = new ExerciseConfiguration(
                config,
                (EvaluationResults results) =>
                {
                    AIProxy aiProxy = new ArmAIProxy(); // should be taken from context
                    ArticolationError elbowError = aiProxy.UnwrapFromResults("gomito", results);
                }
                );

            // to add more events handler:
            exerciseConfiguration.OnExecutionStepEvaluated += (EvaluationResults results) => { }; // exercise configuration should be taken from somewhere

            // configure the virtual physioteraphyst
            VirtualPhysioterphyst eval = VirtualPhysioterphyst.Instance;

            // define a timing for the sampling
            float timing = 0.5f, totalDuration = 2f;
            eval.timingBetweenSamples = timing;

            // setup the exercise with the built configuration
            eval.ExerciseSetup(exerciseConfiguration);

            // start recording
            eval.StartSetup();

            // Start playing a new exercise (guided by the real physioteraphyst)
            Sequence sequence = DOTween.Sequence();
            sequence.Append(hand.transform.DOMove(hand.transform.position * 2, totalDuration));

            // once finished the exercise, stop setup
            sequence.OnComplete(() => {
                eval.EndSetup(); // stop registering
                // ... evaluating if the movement has to be saved or discarded
                eval.SaveSetup(); // save registration
                // eval.DiscardSetup(); // discard registration
            });

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
