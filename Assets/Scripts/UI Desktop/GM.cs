using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Desktop;

public class GM : MonoBehaviour {

    public GameObject UIManager;

    PatientProfile pf;

    void Start() {

        pf = new PatientProfile() {
            IDPatient = "MG",
            NamePatient = "Marco",
            SurnamePatient = "Rossi",
            GenderPatient = "Male",
            HeightPatient = 1.7f,
            DisabilityPatient = "Broken shoulder",
            AgonisticPatient = true
        };

        UIManager.GetComponent<UIManager>().ActiveSelectionPatientPanel(pf);
	}

    public PatientProfile GetPatient() {
        return pf;
    }
}
