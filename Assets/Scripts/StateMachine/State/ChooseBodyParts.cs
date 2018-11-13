using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Physiotherapy.StateMachine
{
    public class ChooseBodyParts : BaseState
    {

        AppFlowContext myContext;

        public override void Enter()
        {
            UIDesktopManager.EventBodyPartSelected += BodyPartChosen;


            myContext = (AppFlowContext)context;
            myContext.listBodyParts = new List<BodyPart>() ;

            BodyPart[] bodyParts = Resources.LoadAll<BodyPart>("BodyPartScriptableObj");

            myContext.listBodyParts = bodyParts.ToList();
            UIDesktopManager.I.ActiveSelectionBodyPartPanel(myContext.listBodyParts);


            base.Enter();
        }


        public override void Tick()
        {

            if (Input.GetKeyDown(KeyCode.A))
                myContext.DoneCallBack();
        }

        public override void Exit()
        {
            UIDesktopManager.EventBodyPartSelected -= BodyPartChosen;
        }

        public void BodyPartChosen(BodyPart bp)
        {
            myContext.currentBodyPart = bp;
            myContext.DoneCallBack();
        }
    }
}

