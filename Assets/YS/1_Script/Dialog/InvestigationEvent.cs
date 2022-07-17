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
        public uint nextIndex;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.ivStruct.investigationCharacter.sprite = gm.charImgs[(int)character];
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