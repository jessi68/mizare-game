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
        [Tooltip("조사이벤트가 끝난 후 이동할 이벤트 번호")]
        public int nextIndex;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.ivStruct.Setup(character, nextIndex);
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