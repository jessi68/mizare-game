using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class InferenceEvent : BaseScriptEvent
    {
        #region Fields
        [SerializeField]
        [LabelText("�߸��� ������ ĳ����")]
        private CHARACTER_IMAGE_INDEX charIndex;
        [SerializeField]
        [LabelText("�߸��� ������")]
        private ITEM_INDEX itemIndex;
        [SerializeField]
        [LabelText("��������")]
        private InferenceDialogData[] choiceDatas;
        [SerializeField]
        [LabelText("Ʋ�ȴ� ������ ���ý� ������ ����")]
        private string twiceFailStr;
        [SerializeField]
        [LabelText("�߸� �̺�Ʈ�� ���� �� �̵��� �̺�Ʈ ��ȣ")]
        private int nextIndex;
        #endregion

        #region Properties
        public CHARACTER_IMAGE_INDEX CharacterIndex => charIndex;
        public ITEM_INDEX ItemIndex => itemIndex;
        public InferenceDialogData[] ChoiceDatas => choiceDatas;
        public int NextIndex => nextIndex;
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();

            gm.ifStruct.Setup(this);
        }
        protected override void OnUpdate()
        {
        }
        public override void OnExit()
        {
            gm.ifStruct.Release();

            base.OnExit();
        }
    }
    [System.Serializable]
    public struct InferenceDialogData
    {
        [LabelText("������ ����")]
        public string choiceStr;
        [LabelText("���� �� ��ȭ����")]
        public DialogEvent[] dialogs;
    }
}