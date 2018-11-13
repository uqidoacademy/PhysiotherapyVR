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

    private LimbPartScript limbPartScript;

    private List<GameObject> LimbPartList;

    public delegate void ButtonProfileSelectedClicked(PatientProfile pp);
    public static ButtonProfileSelectedClicked EventProfileSelected;

    public delegate void ButtonBodyPartClicked(BodyPart pp);
    public static ButtonBodyPartClicked EventBodyPartSelected;

    public delegate void ButtonWearButtonClicked();
    public static ButtonWearButtonClicked EventWearButtonClicked;

    public delegate void SetUpButtonClicked();
    public static SetUpButtonClicked EventSetUpTrackers;

    private static UIDesktopManager instance;

    private TrackerManager trackerManager;

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

    public GameObject SetupTrackersPanel;

    GameObject patientButton;

    GameObject bodyPartButton;

    GameObject wearTrackerReadyButton;

    GameObject trackerStatus;

    GameObject limbPart;

    public void ActiveSelectionPatientPanel(List<PatientProfile> listPatient) {
        SelectionPatientPanel.SetActive(true);
        foreach (PatientProfile pf in listPatient) {
            patientButton = Instantiate(Resources.Load("UIPrefabs/PatientProfileButton")) as GameObject;
            patientButton.GetComponentInChildren<PatientProfileButton>().PatientProfileObj = pf;
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
            bodyPartButton.GetComponentInChildren<BodyPartButton>().BodyPartObj = bp;
            bodyPartButton.transform.parent = SelectionBodyPartPanel.transform.GetChild(0).GetChild(0).GetChild(0);
        }
    }

    public void ActiveWearTrakersPanel() {

        SelectionBodyPartPanel.SetActive(false);
        WearTrackersPanel.SetActive(true);
        WearButtonReady();
    }

    void WearButtonReady()
    {
        trackerStatus = Instantiate(Resources.Load("UIPrefabs/WearTrackerStatusText")) as GameObject;
        trackerStatus.transform.parent = WearTrackersPanel.transform.GetChild(0).GetChild(0).GetChild(0);
        wearTrackerReadyButton = Instantiate(Resources.Load("UIPrefabs/WearButton")) as GameObject;
        wearTrackerReadyButton.GetComponentInChildren<Text>().text = "Start setup trakers";
        wearTrackerReadyButton.transform.parent = WearTrackersPanel.transform.GetChild(0).GetChild(0).GetChild(0);

        
        trackerManager = FindObjectOfType<TrackerManager>();

        if (trackerManager.setUpTrackerDone)
        {
            wearTrackerReadyButton.GetComponent<Button>().interactable = true;
            trackerStatus.GetComponentInChildren<Text>().text = "Devices are ready";
        }
        else
        {
            wearTrackerReadyButton.GetComponent<Button>().interactable = false;
            trackerStatus.GetComponentInChildren<Text>().text = "Devices are not ready";
        }
    }

    public void WearButtonClicked()
    {
        EventWearButtonClicked();
    }

    public void ActiveSetupTrackersPanel(BodyPart bp)
    {
        LimbPartList = new List<GameObject>();
        WearTrackersPanel.SetActive(false);
        SetupTrackersPanel.SetActive(true);
        foreach (string lp in bp.LimbPart)
        {
            
            limbPart = Instantiate(Resources.Load("UIPrefabs/LimbPart")) as GameObject;
            limbPart.GetComponent<LimbPartScript>().LimbPartName = lp;
            limbPart.GetComponent<LimbPartScript>().SetColor(Color.red);
            limbPart.transform.parent = SetupTrackersPanel.transform.GetChild(0).GetChild(0).GetChild(0);
            LimbPartList.Add(limbPart);
        }
    }

    public void LimbPartReady(string LimbPartName)
    {
        foreach (GameObject lp in LimbPartList)
        {
            limbPartScript = lp.GetComponent<LimbPartScript>();
            if (limbPartScript.LimbPartName == LimbPartName)
            {
                lp.GetComponent<LimbPartScript>().SetColor(Color.green);
                LimbPartList.Remove(lp);
            }
        }
        if (LimbPartList.Count == 0)
        {
            EventSetUpTrackers();
        }
    }
}