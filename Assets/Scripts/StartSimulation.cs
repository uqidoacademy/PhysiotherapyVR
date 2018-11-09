using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartSimulation : MonoBehaviour {

	public void StartMatrix(){
		GetComponent<PlayableDirector>().Play();
	}
}
