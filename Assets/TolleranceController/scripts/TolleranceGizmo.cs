using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TolleranceGizmo : MonoBehaviour {
    [HideInInspector]
    [Range(0, 50)]
    public int segments = 30;
    public Color BaseTransparentShapeColor = Color.white;
    [Range(0, 5)]
    public float radius = 2;
    [HideInInspector]
    public float shapeOffset = 0.1f;
    [HideInInspector]
    public Transform TransparentShape;


    CyrcleLine[] cyrcleLines;

	// Use this for initialization
	void Start () {
        cyrcleLines = GetComponentsInChildren<CyrcleLine>();
        TransparentShape = gameObject.GetComponentsInChildren<Transform>().First(t => t.name == "TransparentMesh");
        DrawTollerances();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DrawTollerances();
            DrawTolleranceGizmo(BaseTransparentShapeColor);
        }
	}

    void DrawTollerances() {
        foreach (var c in cyrcleLines) {
            c.segments = segments;
            c.xradius = radius;
            c.yradius = radius;
            c.CreatePoints();
            TransparentShape.localScale = new Vector3(radius * 2, radius * 2, radius * 2) - new Vector3(shapeOffset, shapeOffset, shapeOffset);
        }
    }

    #region API

    /// <summary>
    /// Refresh tollerance view Gizmo.
    /// </summary>
    public void DrawTolleranceGizmo() {
        DrawTolleranceGizmo(BaseTransparentShapeColor);
    }

    /// <summary>
    /// Refresh tollerance view Gizmo.
    /// </summary>
    /// <param name="_color">Color to apply to semi-transparent shape. Alpha well be ignored.</param>
    public void DrawTolleranceGizmo(Color _color) {
        DrawTollerances();
        TransparentShape.GetComponent<MeshRenderer>().material.color = new Color(_color.r, _color.g, _color.b, 0.1f);
    }

    #endregion
}
