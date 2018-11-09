using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BodyPart", menuName = "BodyParts/BodyPart", order = 1)]
public class BodyPart : ScriptableObject {
	public string PartName;
	public float NumberTrackers;
}
