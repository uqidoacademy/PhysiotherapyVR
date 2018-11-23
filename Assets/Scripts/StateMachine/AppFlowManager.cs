using Physiotherapy.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Desktop;

public class AppFlowManager : MonoBehaviour
{
    public TrackerRenamer trackerRenamer;

    public Text DebugText;

    public MyStateMachine sm;

    List<IState> states;

    AppFlowContext context;

    private static AppFlowManager _instance;

    public static AppFlowManager I
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    void Start()
    {
        // Setup SM
        sm = gameObject.AddComponent<MyStateMachine>();

        context = new AppFlowContext()
        {
            DebugText = DebugText,
            PatientSelectionDoneCallback = patientSelectionDone,
            DoneCallBack = goToNextState,
            TrackerWearDoneCallback = goToSetUpTracker,
            SetUpTrackerDoneCallback = goToRegistrationSample,
            RetryRegistrationCallback = goToRegistrationSample,
            RegistrationOkCallback = goToExerciseReady,
            FinishExerciseSameBdPartCallback = goToRegistrationSample,
            FinishExerciseDifferentBdPartCallback = goToChooseBodyParts,

            // ChooseBodyPartsDoneCallback = patientSelectionToDo,
        };

        states = new List<IState>() {
            new PatientSelection().Setup(context),
            new ChooseBodyParts().Setup(context),
            new WearTracker().Setup(context),
            new SetUpTracker().Setup(context),
            new RegistrationSample().Setup(context),
            new ExerciseReady().Setup(context),
            new DoingExercise().Setup(context),
            new EndExercise().Setup(context),

        };
        sm.Setup(context, states);

        patientSelectionToDo();
    }

    



    #region routes

    private void patientSelectionToDo() {
        Debug.Log("First State");
        sm.CurrentState = sm.States[0];
    }

    private void patientSelectionDone()
    {

        Debug.Log("Second State");
        sm.CurrentState = getState(typeof(ChooseBodyParts));
    }



    /// <summary>
    /// Usata tutte le volte che semplicemente si passa allo stato successivo seguendo il normale flow dell'evento
    /// </summary>
    private void goToNextState()
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i] == sm.CurrentState)
            {
                if (i + 1 == states.Count)
                    sm.CurrentState = states[0];
                else
                    sm.CurrentState = states[i + 1];
                break;
            }

        }
    }

    private void goToChooseBodyParts()
    {
        if (context.currentPatient != null)
        {
            sm.CurrentState = getState(typeof(ChooseBodyParts));
        }
    }

    private void goToSetUpTracker()
    {
        if (context.currentPatient != null )
        {
         
            sm.CurrentState = getState(typeof(SetUpTracker));
        }
    }

    private void goToRegistrationSample()
    {
        if (context.currentPatient != null && context.currentBodyPart != null)
        {
            sm.CurrentState = getState(typeof(RegistrationSample));
        }
    }

    private void goToExerciseReady()
    {
        if (context.currentPatient != null && context.currentBodyPart != null)
        {
            sm.CurrentState = getState(typeof(ExerciseReady));
        }
    }

    private IState getState(Type _state)
    {
        foreach (IState state in states)
        {
            if (state.GetType() == _state)
                return state;
        

        }

        return null;
    }

   


    #endregion

    //void OnStart()
    //{
    //    sm.CurrentState = sm.States[0];
    //}

    //void OnSetupDone()
    //{
    //    sm.CurrentState = sm.States[1];
    //}
}


public class AppFlowContext : IContext
{
    public List<PatientProfile> ListPatient;

    public PatientProfile currentPatient;

    public List<BodyPart> listBodyParts;

    public BodyPart currentBodyPart;

    public TrackerManager trackerManager;

    public Text DebugText;

    /// <summary>
    /// Contiene la callBack da richiamare quando è finita la selezione paziente
    /// </summary>
    public Action PatientSelectionDoneCallback;

    /// <summary>
    /// Contiene la callback da richiamare quando è stata fatta la scelta delle parti del corpo
    /// </summary>
    public Action ChooseBodyPartsDoneCallback;

    /// <summary>
    /// COntiene la callback da richiamare quando sono stati indossati correttamente i tracker
    /// </summary>
    public Action TrackerWearDoneCallback;

    public Action SetUpTrackerDoneCallback;

    /// <summary>
    /// callback da richiamare quando si vuole rifare la registrazione
    /// </summary>
    public Action RetryRegistrationCallback;

    public Action RegistrationOkCallback;

    /// <summary>
    /// callback da richiamare finito l'esercizio se si vuole farne un altro con la stessa parte del corpo coinvolta
    /// </summary>
    public Action FinishExerciseSameBdPartCallback;

    /// <summary>
    /// callback da richiamare finito l'esercizio se si vuole farne un altro con diversa parte del corpo coinvolta
    /// </summary>
    public Action FinishExerciseDifferentBdPartCallback;

    /// <summary>
    /// Callback generica da richiamare quando finisco il lavoro dello stato
    /// </summary>
    public Action DoneCallBack;

    

    public Text GetDebugText()
    {
        return DebugText;
    }
}