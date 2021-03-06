﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physiotherapy.StateMachine
{
    public class WearTracker : BaseState
    {

        AppFlowContext myContext;

        TrackerManager tm;
        public override void Enter()
        {
            UIDesktopManager.EventWearButtonClicked += TrackerWorn;
            myContext = (AppFlowContext)context;
            base.Enter();
            tm = GameObject.FindObjectOfType<TrackerManager>();
            tm.SetUpTrackers(myContext.currentBodyPart.LimbPart);
            UIDesktopManager.I.ActiveWearTrakersPanel();
            myContext.trackerManager = tm;
        }

        public void TrackerWorn() {
            myContext.DoneCallBack();
        }


        public override void Tick()
        {
            /*
            if (Input.GetKeyDown(KeyCode.A))
                myContext.DoneCallBack();
            if (tm.setUpTrackerDone)
                myContext.TrackerWearDoneCallback();*/
        }

        public override void Exit()
        {
            UIDesktopManager.I.WearTrackersPanel.SetActive(false);
            UIDesktopManager.EventWearButtonClicked -= TrackerWorn;
        }
    }
}
