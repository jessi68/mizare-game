using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class InvestigationEvent : BaseScriptEvent
    {
        [Tooltip("조사이벤트가 끝난 후 이동할 이벤트 번호")]
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