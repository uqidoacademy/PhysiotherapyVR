
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TriggerRecord : MonoBehaviour
{
    public SteamVR_Action_Boolean recordAction;

    private SenderExerciseAI recorder;

    private Hand hand;

    private void OnEnable()
    {
        if (hand == null)
            hand = this.GetComponent<Hand>();

        recorder = FindObjectOfType<SenderExerciseAI>();

        if (recordAction == null)
        {
            Debug.LogError("No action assigned");
            return;
        }

        recordAction.AddOnChangeListener(OnTriggerActionChange, hand.handType);
    }

    private void OnDisable()
    {
        if (recordAction != null)
            recordAction.RemoveOnChangeListener(OnTriggerActionChange, hand.handType);
    }

    private void OnTriggerActionChange(SteamVR_Action_In actionIn)
    {
        if (recordAction.GetStateDown(hand.handType))
        {
            Grip();
        }

        if (recordAction.GetStateUp(hand.handType))
        {
            LoseGrip();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
            Grip();
        if (Input.GetKeyUp("space"))
            LoseGrip();

    }

    private void Grip()
    {
        StartCoroutine(StartRecording());
    }

    private void LoseGrip()
    {
        StartCoroutine(StopRecording());
    }

    private IEnumerator StartRecording()
    {
        recorder.StartSendRecording();
        yield return null;
    }

    private IEnumerator StopRecording()
    {
        recorder.StopSendRecording();
        yield return null;
    }
}