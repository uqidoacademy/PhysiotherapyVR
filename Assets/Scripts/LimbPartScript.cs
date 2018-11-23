using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LimbPartScript : MonoBehaviour {

    string limbPartName;

    public Text text;

    public Image img;

    public string LimbPartName
    {
        get { return limbPartName; }
        set
        {
            limbPartName = value;
            text.text = value;
        }
    }

    public void SetColor(Color color) {
        img.color = color;
    }
}
