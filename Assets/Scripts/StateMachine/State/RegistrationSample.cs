using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physiotherapy.StateMachine
{
    public class RegistrationSample : BaseState
    {

        AppFlowContext myContext;

        public override void Enter()
        {

            myContext = (AppFlowContext)context;

            SenderExerciseAI senderToCreate = GameObject.FindObjectOfType<SenderExerciseAI>();
            senderToCreate.shoulder = myContext.trackerManager.trackerListReady[0].reference;
            senderToCreate.elbow = myContext.trackerManager.trackerListReady[1].reference;
            senderToCreate.hand = myContext.trackerManager.trackerListReady[2].reference;

            base.Enter();
        }


        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.A))
                myContext.DoneCallBack();

        }

        public override void Exit()
        {

        }
    }
}
