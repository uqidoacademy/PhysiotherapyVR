using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "BodyParts/BodyPart", order = 1)]
public class BodyPart : ScriptableObject {
	public string PartName;

    public Sprite icon;

	public int NumberTrackers;

	public float PositionTollerance;

	public float VelocityTollernace; 
}
