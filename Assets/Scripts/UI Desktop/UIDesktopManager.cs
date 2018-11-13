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

    public delegate void ButtonProfileSelectedClicked(PatientProfile pp);
    public static ButtonProfileSelectedClicked EventProfileSelected;

    public delegate void ButtonBodyPartClicked(BodyPart pp);
    public static ButtonBodyPartClicked EventBodyPartSelected;

    public delegate void ButtonWearButtonClicked();
    public static ButtonWearButtonClicked EventWearButtonClicked;

    Dictionary<string, Image> colorizers = new Dictionary<string, Image>();

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
        
        wearTrackerReadyButton = Instantiate(Resources.Load("UIPrefabs/WearButton")) as GameObject;
        wearTrackerReadyButton.GetComponentInChildren<Text>().text = "Start selection body parts";
        wearTrackerReadyButton.transform.parent = WearTrackersPanel.transform.GetChild(0).GetChild(0).GetChild(0);

        trackerStatus = Instantiate(Resources.Load("UIPrefabs/WearTrackerStatusText")) as GameObject;
        trackerStatus.transform.parent = WearTrackersPanel.transform.GetChild(0).GetChild(0).GetChild(0);
        trackerManager = FindObjectOfType<TrackerManager>();

        if (trackerManager.setUpTrackerDone == false)
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
        Debug.Log("Ciao!");
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

    public void ActiveTrackersFeedbackPanel(BodyPart bp, ExerciseConfiguration configuration)
    {
        SetupTrackersPanel.SetActive(false);
        TrackersFeedbackPanel.SetActive(true);

        colorizers.Clear();

        foreach (string lp in bp.LimbPart)
        {
            limbPart = Instantiate(Resources.Load("UIPrefabs/LimbPart")) as GameObject;
            limbPart.GetComponentInChildren<Text>().text = lp;
            Image colorizer = limbPart.transform.GetChild(0).GetComponent<Image>();
            colorizer.color = Color.yellow;
            colorizers[lp] = colorizer;
            limbPart.transform.parent = TrackersFeedbackPanel.transform.GetChild(0).GetChild(0).GetChild(0);
        }
        StopTrackingFeedback(configuration);
        configuration.OnExecutionStepEvaluated += HandleFeedbackResults;
    }

    public void StopTrackingFeedback(ExerciseConfiguration configuration)
    {
        configuration.OnExecutionStepEvaluated -= HandleFeedbackResults;
    }

    private void HandleFeedbackResults(EvaluationResults results)
    {
        AIProxy aiProxy;

        // TODO: get bodypart type
        if (true)
        {
            // ARM
            aiProxy = new ArmAIProxy();
        }

        List<string> limbsIDs = StaticTestList.ArtList;
        foreach (string limbID in limbsIDs)
        {
            ArticolationError limbError = aiProxy.UnwrapFromResults(limbID, results);
            colorizers[limbID].color = limbError.isCorrect ? Color.green : Color.red;
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
            }
        }
    }
}