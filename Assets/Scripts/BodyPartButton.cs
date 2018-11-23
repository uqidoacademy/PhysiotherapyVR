using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartButton : MonoBehaviour {

    BodyPart bp;
    public Text text;
    public Image img;

    public BodyPart BodyPartObj {
        get { return bp; }
        set {
            bp = value;
            text.text = bp.name;
            img.sprite = bp.icon;
        }
    }

    public void ButtonClicked()
    {
        UIDesktopManager.EventBodyPartSelected(bp);
    }
}
