﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physiotherapy.StateMachine
{
    public class SetUpTracker : BaseState
    {

        AppFlowContext myContext;
        TrackerRenamer trackerRenamer;
        public override void Enter()
        {
            myContext = (AppFlowContext)context;
            base.Enter();
            UIDesktopManager.EventSetUpTrackers += SetUpTrackersDone;
            trackerRenamer = GameObject.FindObjectOfType<TrackerRenamer>();
            UIDesktopManager.I.ActiveSetupTrackersPanel(myContext.currentBodyPart);
            trackerRenamer.StartRename(myContext.currentBodyPart.LimbPart);

        }


        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.A))
                myContext.DoneCallBack();
        }

        public override void Exit()
        {
            UIDesktopManager.EventSetUpTrackers -= SetUpTrackersDone;
            UIDesktopManager.I.SetupTrackersPanel.SetActive(false);
        }

        public void SetUpTrackersDone()
        {
            myContext.DoneCallBack();
        }
    }
}
