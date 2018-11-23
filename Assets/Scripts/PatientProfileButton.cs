using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Desktop;
using UnityEngine.UI;

public class PatientProfileButton : MonoBehaviour {

    PatientProfile pp;

    public Text text;

	public PatientProfile PatientProfileObj {
            get { return pp; }
            set { pp = value;
            text.text = pp.NamePatient + " " + pp.SurnamePatient;
            }
    }

    public void ButtonClicked() {
        UIDesktopManager.EventProfileSelected(pp);
    }
}
