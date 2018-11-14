using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestChangeColor : MonoBehaviour {

  public GameObject uivr;
	// Use this for initialization
	void Start () {
    Canvas trackerOb = uivr.GetComponentInChildren<Canvas>();

    trackerOb.GetComponentInChildren<Image>().color = Color.green;

  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
