using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class InvestigationEvent : BaseScriptEvent
    {
        [SerializeField]
        [Tooltip("조사를 진행할 캐릭터")]
        private CHARACTER_IMAGE_INDEX character;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.ivStruct.Setup(character);
        }
        protected override void OnUpdate()
        {
            gm.ivStruct.OnUpdate();
        }
        public override void OnExit()
        {
            gm.ivStruct.Release();

            base.OnExit();
        }
    }
}