/* CLASSE BodyPart
 * Classe che permette di creare un ScriptableObj
 * per una parte del corpo
*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "BodyParts/BodyPart", order = 1)]
public class BodyPart : ScriptableObject {
	public string PartName;

    public Sprite icon;

	public int NumberTrackers;

    public List<string> LimbPart;

	public float PositionTollerance;

	public float VelocityTollernace; 
}
