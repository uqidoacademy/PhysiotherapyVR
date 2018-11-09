using Limb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public KeyCode key = KeyCode.Space;
    public float timing = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(key))
        {
            Debug.Log("START TEST");
            foreach(LimbConfiguration config in Evaluator.Instance.configs)
            {
                Debug.Log("For limb config " + config.limb + " found " + config.sensors.Count + "sensors");
            }

            // Start play ideal

            Evaluator.Instance.ExerciseSetup(timing);

            // End play ideal

            Evaluator.Instance.SaveSetup();

            // Start play real

            Evaluator.Instance.StartEvaluation();

            // End play real

            Evaluator.Instance.StopEvaluation();



        }
	}
}
