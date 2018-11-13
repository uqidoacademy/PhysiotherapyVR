using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Desktop;
using UnityEngine.UI;

namespace Physiotherapy.StateMachine
{
    public class PatientSelection : BaseState 
    {

        public UnityEngine.UI.Button button;

        AppFlowContext myContext;

        

        public override void Enter()
        {
            UIDesktopManager.EventProfileSelected += DoneSelectionPatient;

            myContext = (AppFlowContext)context;

            myContext.ListPatient = new List<PatientProfile>();
            myContext.ListPatient.Add(new PatientProfile("MR", "Mario",
                "Rossi", "Male", 1.7f, true, "Broken shoulder"));
            myContext.ListPatient.Add(new PatientProfile("SN", "Simone",
               "Niero", "Male", 1.7f, true, "Broken shoulder"));
            UIDesktopManager.I.ActiveSelectionPatientPanel(myContext.ListPatient);

            




            base.Enter();
        }

        
        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.A))
                myContext.DoneCallBack();
        }

        public void DoneSelectionPatient(PatientProfile pp)
        {
            //TODO: mettere il paziente selezionato, non il primo della lista
            myContext.currentPatient = pp;

            myContext.DoneCallBack();
        }

        public override void Exit()
        {
            UIDesktopManager.EventProfileSelected -= DoneSelectionPatient;
        }
    }
}
