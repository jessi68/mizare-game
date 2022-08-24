using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class InvestigationEvent : BaseScriptEvent
    {
        [SerializeField]
        [Tooltip("���縦 ������ ĳ����")]
        private Sprite character;

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