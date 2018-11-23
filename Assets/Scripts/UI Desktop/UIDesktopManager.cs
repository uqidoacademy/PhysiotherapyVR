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
using VRPhysiotheraphyst;
using AI.Results;
using AI.Proxy;
using AI.Error;

public class UIDesktopManager : MonoBehaviour {

    private LimbPartScript limbPartScript;

    private List<GameObject> LimbPartList;

    #region Events

    public delegate void ButtonProfileSelectedClicked(PatientProfile pp);
    public static ButtonProfileSelectedClicked EventProfileSelected;

    public delegate void ButtonBodyPartClicked(BodyPart pp);
    public static ButtonBodyPartClicked EventBodyPartSelected;

    public delegate void ButtonClicked();

    public static ButtonClicked EventWearButtonClicked;

    public static ButtonClicked EventSetUpTrackers;

    public static ButtonClicked EventRetryRegistration;

    public static ButtonClicked EventGoToExercise;

    public static ButtonClicked EventEndExperience;

    public static ButtonClicked EventReDoExerciseSameBodyPart;

    public static ButtonClicked EventReDoExerciseDifferentBodyPart;

    #endregion

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

    public GameObject TrackersFeedbackPanel;

    public GameObject RegistrationExercisePanel;

    GameObject patientButton;

    GameObject bodyPartButton;

    GameObject wearTrackerReadyButton;

    GameObject trackerStatus;

    GameObject limbPart;

    Dictionary<string, Image> colorizers = new Dictionary<string, Image>();

    public void ActiveSelectionPatientPanel(List<PatientProfile> listPatient) {
        SelectionPatientPanel.SetActive(true);


        if (SelectionPatientPanel.transform.Find("ScrollView/Viewport/Content").childCount <= 0)
        {
            foreach (PatientProfile pf in listPatient)
            {
                patientButton = Instantiate(Resources.Load("UIPrefabs/PatientProfileButton")) as GameObject;
                patientButton.GetComponentInChildren<PatientProfileButton>().PatientProfileObj = pf;
                patientButton.transform.parent = SelectionPatientPanel.transform.GetChild(0).GetChild(0).GetChild(0);
            }
        }
    }

    public void ActiveSelectionBodyPartPanel(List<BodyPart> listBodyPart)
    {
       
        SelectionBodyPartPanel.SetActive(true);
        if (SelectionBodyPartPanel.transform.Find("ScrollView/Viewport/Content").childCount <= 0)
        {
            foreach (BodyPart bp in listBodyPart)
            {
                bodyPartButton = Instantiate(Resources.Load("UIPrefabs/BodyPartButton")) as GameObject;
                bodyPartButton.GetComponentInChildren<BodyPartButton>().BodyPartObj = bp;
                bodyPartButton.transform.parent = SelectionBodyPartPanel.transform.GetChild(0).GetChild(0).GetChild(0);
            }
        }
    }

    public void ActiveWearTrakersPanel() {

        
        WearTrackersPanel.SetActive(true);
        if (WearTrackersPanel.transform.Find("Panel/Viewport/Content").childCount <= 0)
        {
            WearButtonReady();
        }
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
            trackerStatus.GetComponentInChildren<Text>().color = Color.green;
        }
        else
        {
            wearTrackerReadyButton.GetComponent<Button>().interactable = false;
            trackerStatus.GetComponentInChildren<Text>().text = "Devices are not ready";
            trackerStatus.GetComponentInChildren<Text>().color = Color.red;
        }
    }

    public void WearButtonClicked()
    {
        EventWearButtonClicked();
    }

    public void ActiveSetupTrackersPanel(BodyPart bp)
    {
        LimbPartList = new List<GameObject>();
        SetupTrackersPanel.SetActive(true);
        if (SetupTrackersPanel.transform.Find("Panel/Viewport/Content").childCount <= 0)
        {
            foreach (string lp in bp.LimbPart)
            {

                limbPart = Instantiate(Resources.Load("UIPrefabs/LimbPart")) as GameObject;
                limbPart.GetComponent<LimbPartScript>().LimbPartName = lp;
                limbPart.GetComponent<LimbPartScript>().SetColor(Color.red);
                limbPart.transform.parent = SetupTrackersPanel.transform.GetChild(0).GetChild(0).GetChild(0);
                LimbPartList.Add(limbPart);
            }
        }
    }

    void ColorLimbAIResult(string limb,bool correctPosition){
        colorizers[limb].color = correctPosition ? Color.green : Color.red;
    }

    public void ActiveTrackersFeedbackPanel(BodyPart bp)
    {
        SenderExerciseAI.EventSendResultAI += ColorLimbAIResult;

        ExerciseConfiguration configuration = FindObjectOfType<SenderExerciseAI>().exerciseConfiguration;
        TrackersFeedbackPanel.SetActive(true);
        
        colorizers.Clear();
        if (TrackersFeedbackPanel.transform.Find("Panel/Viewport/Content").childCount <= 0)
        {
            foreach (string lp in bp.LimbPart)
            {
                limbPart = Instantiate(Resources.Load("UIPrefabs/LimbPart")) as GameObject;
                limbPart.GetComponentInChildren<Text>().text = lp;
                Image colorizer = limbPart.transform.GetChild(0).GetComponent<Image>();
                colorizer.color = Color.yellow;
                colorizers[lp] = colorizer;
                Transform parent = TrackersFeedbackPanel.transform.GetChild(0).GetChild(0).GetChild(0);
                limbPart.transform.parent = parent;
            }
        }
        //StopTrackingFeedback(bp.LimbPart, configuration);
    }

    List<string> ArmListIDs = new List<string>();
    public void StopTrackingFeedback(List<string> limbIDs, ExerciseConfiguration configuration)
    {
        ArmListIDs = limbIDs;
    }

    public void LimbPartReady(string LimbPartName)
    {
        for(int i=0; i < LimbPartList.Count; i++)
        {
            limbPartScript = LimbPartList[i].GetComponent<LimbPartScript>();
            if (limbPartScript.LimbPartName == LimbPartName)
            {
                LimbPartList[i].GetComponent<LimbPartScript>().SetColor(Color.green);
                LimbPartList.Remove(LimbPartList[i]);
            }
        }
        if (LimbPartList.Count == 0)
        {
            if(EventSetUpTrackers != null)
            EventSetUpTrackers();
        }
    }

    public void ActiveRegistrationExercisePanel() {
       
        RegistrationExercisePanel.SetActive(true);
    }

    #region Button functions

    public void RetryRegistration() {
        EventRetryRegistration();
    }

    public void GoToExercise() {
        EventGoToExercise();
    }

    public void RegistrationFeedback(bool registrationState) {
        if (registrationState)
        {
            RegistrationExercisePanel.transform.Find("Panel/Viewport/Content/RegistrationText").GetComponent<Text>().text = "In registration";
            RegistrationExercisePanel.transform.Find("Panel/Viewport/Content/RegistrationText").GetComponent<Text>().color = Color.green;
        }
        else {
            RegistrationExercisePanel.transform.Find("Panel/Viewport/Content/RegistrationText").GetComponent<Text>().text = "Not in registration";
            RegistrationExercisePanel.transform.Find("Panel/Viewport/Content/RegistrationText").GetComponent<Text>().color = Color.red;
        }
    }

    public void EndExperience()
    {
        EventEndExperience();
    }

    public void ReDoExerciseSameBodyPart()
    {
        EventReDoExerciseSameBodyPart();
    }

    public void ReDoExerciseDifferentBodyPart()
    {
        EventReDoExerciseDifferentBodyPart();
    }

    #endregion
}