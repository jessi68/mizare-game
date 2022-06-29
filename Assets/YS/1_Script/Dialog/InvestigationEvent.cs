using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class InvestigationEvent : BaseScriptEvent
    {
        public uint nextIndex;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.ivStruct.SetInvestigationMode(nextIndex);
        }
        protected override void OnUpdate()
        {
            gm.ivStruct.OnUpdate();
        }
        public override void OnExit()
        {
            gm.ivStruct.investigationUI.SetActive(false);

            base.OnExit();
        }
    }
}