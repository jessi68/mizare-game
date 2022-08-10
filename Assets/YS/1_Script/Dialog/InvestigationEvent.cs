using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class InvestigationEvent : BaseScriptEvent
    {
        [SerializeField]
        [Tooltip("���縦 ������ ĳ����")]
        private CHARACTER_IMAGE_INDEX character;
        [Tooltip("�����̺�Ʈ�� ���� �� �̵��� �̺�Ʈ ��ȣ")]
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