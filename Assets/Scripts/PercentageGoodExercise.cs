using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageGoodExercise : MonoBehaviour {

	public delegate void SendPercentageChanged (int percentageInt);
	public static SendPercentageChanged EventSendPercentageExercise;

	private float numberOfTakenSamples;

	private float totalSumOfCorrectSamples;

	private float cooldownTick;

	public float tickCadence;

	// Use this for initialization
	void Start () {
		Reset ();
	}

	public void Reset () {
		numberOfTakenSamples = 0;
		totalSumOfCorrectSamples = 0;
		cooldownTick = 0;
	}

	// Update is called once per frame
	void Update () {
		cooldownTick -= Time.deltaTime;
		if (cooldownTick <= 0)
			SendPercentageExercise ();
	}

	void SendPercentageExercise () {
		if (numberOfTakenSamples != 0) {
			int percentage = Mathf.RoundToInt (totalSumOfCorrectSamples / numberOfTakenSamples * 100);
			if (EventSendPercentageExercise != null)
				EventSendPercentageExercise (percentage);
		}
	}

	void ExerciseResult (string limb, bool correctPosition) {
		numberOfTakenSamples += 1;
		if (correctPosition)
			totalSumOfCorrectSamples += 1;
	}

}