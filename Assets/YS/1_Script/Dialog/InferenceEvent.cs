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
        [BoxGroup("Ʋ�ȴ� ������ ���ý� ������ ����", true, true), SerializeField]
        [HideLabel]
        private DialogEvent twiceFailDialogData = new DialogEvent(false);
        [SerializeField, MaxValue("@choiceDatas.Length - 1")]
        [LabelText("����"), Tooltip("�߸� ���������� �ùٸ� ��")]
        private uint correctIndex;
        [SerializeField]
        [LabelText("�߸� �̺�Ʈ�� ���� �� �̵��� �̺�Ʈ ��ȣ")]
        private int nextIndex;
        #endregion

        #region Properties
        public CHARACTER_IMAGE_INDEX CharacterIndex => charIndex;
        public ITEM_INDEX ItemIndex => itemIndex;
        public InferenceDialogData[] ChoiceDatas => choiceDatas;
        public DialogEvent TwiceFailDialogData => twiceFailDialogData;
        public uint CorrectIndex => correctIndex;
        public int NextIndex => nextIndex;
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();

            gm.ifStruct.Setup(this);
        }
        protected override void OnUpdate()
        {
            gm.ifStruct.OnUpdate();
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
        [ListDrawerSettings(CustomAddFunction = nameof(AddFunction))]
        [LabelText("���� �� ��ȭ����")]
        public DialogEvent[] dialogs;

        private DialogEvent AddFunction()
        {
            return new DialogEvent(false);
        }
    }
}