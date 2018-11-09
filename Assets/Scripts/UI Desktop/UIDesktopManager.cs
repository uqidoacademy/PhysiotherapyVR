using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Desktop;

public class UIDesktopManager : MonoBehaviour {

    private static UIDesktopManager instance;

    public static UIDesktopManager I
    {
        get {
            return instance;
        }
        set {
            instance = value;
        }
    }

    private void Awake() {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }
    }

    public GameObject SelectionPatientPanel;

    public GameObject SelectionBodyPartPanel;

    public void ActiveSelectionPatientPanel(List<PatientProfile> listPatient) {

        SelectionPatientPanel.SetActive(true);
    }
}