/* CLASSE UIDesktopManager
 * Classe che gestisce l'intera UI Desktop
 * permettendo di passare da un pannello all'altro
 * in base allo stato in cui è l'applicazione
*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Desktop;
using Physiotherapy.StateMachine;

public class UIDesktopManager : MonoBehaviour {

    public delegate void ButtonClicked();
    public static ButtonClicked ClickForChangeState;

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

    public GameObject WearTrackersPanel;

    GameObject patientButton;

    GameObject bodyPartButton;

    GameObject limbPart;

    public void ActiveSelectionPatientPanel(List<PatientProfile> listPatient) {
        SelectionPatientPanel.SetActive(true);
        foreach (PatientProfile pf in listPatient) {
            patientButton = Instantiate(Resources.Load("UIPrefabs/PatientProfileButton")) as GameObject;
            patientButton.GetComponentInChildren<Text>().text = pf.NamePatient + " " + pf.SurnamePatient;
            patientButton.transform.parent = SelectionPatientPanel.transform.GetChild(0).GetChild(0).GetChild(0);
        }  
    }

    public void ActiveSelectionBodyPartPanel(List<BodyPart> listBodyPart)
    {
        SelectionPatientPanel.SetActive(false);
        SelectionBodyPartPanel.SetActive(true);
        foreach (BodyPart bp in listBodyPart)
        {
            bodyPartButton = Instantiate(Resources.Load("UIPrefabs/BodyPartButton")) as GameObject;
            bodyPartButton.GetComponentInChildren<Text>().text = bp.name;
            bodyPartButton.GetComponentInChildren<Image>().sprite = bp.icon;
            bodyPartButton.transform.parent = SelectionBodyPartPanel.transform.GetChild(0).GetChild(0).GetChild(0);
        }
    }

    public void ActiveWearTrakersPanel(BodyPart bp) {
        WearTrackersPanel.SetActive(true);
        foreach (string lp in bp.LimbPart) {
            limbPart = Instantiate(Resources.Load("UIPrefabs/LimbPart")) as GameObject;
            limbPart.GetComponentInChildren<Text>().text = lp;
        }
    }

    public void PatientSelected() {
        ClickForChangeState();
    }
}