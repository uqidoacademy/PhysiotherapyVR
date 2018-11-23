using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PercentageExerciseUI : MonoBehaviour {
	
	void Start () {
		PercentageGoodExercise.EventSendPercentageExercise += UpdatePercentageUI;
	}

	void UpdatePercentageUI(int percentageInt){
		GetComponent<TextMeshPro>().text = "Esercizio corretto al "+percentageInt+"%";
	}

}
