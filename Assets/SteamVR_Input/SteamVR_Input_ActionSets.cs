// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Input
    {
        
        public static Valve.VR.SteamVR_Input_ActionSet_default _default;
        
        public static void Dynamic_InitializeActionSets()
        {
            SteamVR_Input._default.Initialize();
        }
        
        public static void Dynamic_InitializeInstanceActionSets()
        {
            Valve.VR.SteamVR_Input._default = ((SteamVR_Input_ActionSet_default)(SteamVR_Input_References.GetActionSet("_default")));
            Valve.VR.SteamVR_Input.actionSets = new Valve.VR.SteamVR_ActionSet[]
            {
                    Valve.VR.SteamVR_Input._default};
        }
    }
}
