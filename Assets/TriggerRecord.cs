
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

    public class TriggerRecord : MonoBehaviour
    {
        public SteamVR_Action_Boolean recordAction;

        public SampleRecorder recorder;

        private Hand hand;

        private void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

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

        if (Input.GetKeyDown(KeyCode.P))
            recorder.StartPlayback();
        if (Input.GetKeyUp(KeyCode.P))
            recorder.StopPlayback();

        
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
        recorder.StartRecording();
        yield return null;
        }

    private IEnumerator StopRecording()
    {
        recorder.StopRecording();
        yield return null;
    }
}