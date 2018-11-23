using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class CyrcleLine : MonoBehaviour {
    [HideInInspector]
    public int segments = 50;
    [HideInInspector]
    public float xradius = 5;
    [HideInInspector]
    public float yradius = 5;
    LineRenderer line;

    public Camera cam;

    void Start() {

    }

    public void CreatePoints() {
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = (segments + 1);
        line.useWorldSpace = false;

        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++) {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, z, 0));

            angle += (360f / segments);
        }
    }



}

