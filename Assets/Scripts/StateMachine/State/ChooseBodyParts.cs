using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physiotherapy.StateMachine
{
    public class ChooseBodyParts : BaseState
    {

        AppFlowContext myContext;

        public override void Enter()
        {
           
            myContext = (AppFlowContext)context;
            myContext.listBodyParts = new List<BodyPart>() ;

            Object[] bodyParts = Resources.LoadAll("BodyPartScriptableObj", typeof(BodyPart));
          

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

