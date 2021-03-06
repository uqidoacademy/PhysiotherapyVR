﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddConnectionParts : MonoBehaviour {

	public GameObject limbPrefab;

	public GameObject userTrackers;

	private float sizeLimb = 0.08f;
	private List<GameObject> limbsTrackers = new List<GameObject> ();

	public List<string> partsOfBody = new List<string>();

    private bool connectLimbs = false;

	// Update is called once per frame
	void Update () {
		if (connectLimbs) {
			for (int i = 0; i < partsOfBody.Count - 1; i++) {
				UpdateCylinderBetweenPoints (limbsTrackers[i].transform.position,
					limbsTrackers[i + 1].transform.position, sizeLimb, transform.GetChild (i).gameObject);
			}
		}
	}

	private void UpdateCylinderBetweenPoints (Vector3 start, Vector3 end, float width, GameObject arto) {
		Vector3 offset = end - start;
		Vector3 scale = new Vector3 (width, offset.magnitude / 2.0f, width);
		Vector3 position = start + (offset / 2.0f);

		arto.transform.position = position;
		arto.transform.rotation = Quaternion.identity;

		arto.transform.up = offset;
		arto.transform.localScale = scale;
	}

	public void PrepareConnections () {
		//partsOfBody = StaticTestList.ArtList;

		//instantiate limbs
		for (int i = 0; i < partsOfBody.Count - 1; i++) {
			Instantiate (limbPrefab, transform);
		}

		for (int i = 0; i < partsOfBody.Count; i++) {
			Transform[] allChildren = userTrackers.GetComponentsInChildren<Transform> ();
			foreach (Transform child in allChildren) {
				if (child.name.Equals (partsOfBody[i])) {
					limbsTrackers.Add (child.gameObject);
				}
			}
		}
		connectLimbs = true;
	}
}