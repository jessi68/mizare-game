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
        private Sprite charIndex;
        [SerializeField]
        [LabelText("�߸��� ������")]
        private ITEM_INDEX itemIndex;
        [SerializeField]
        [LabelText("��������")]
        private InferenceDialogData[] choiceDatas;
        [BoxGroup("Ʋ�ȴ� ������ ���ý� ������ ����", true, true), SerializeField]
        [HideLabel]
        private DialogEvent twiceFailDialogData;
        [SerializeField, MaxValue("@choiceDatas.Length - 1")]
        [LabelText("����"), Tooltip("�߸� ���������� �ùٸ� ��")]
        private uint correctIndex;
        #endregion

        #region Properties
        public Sprite CharacterIndex => charIndex;
        public ITEM_INDEX ItemIndex => itemIndex;
        public InferenceDialogData[] ChoiceDatas => choiceDatas;
        public DialogEvent TwiceFailDialogData => twiceFailDialogData;
        public uint CorrectIndex => correctIndex;
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
        [LabelText("���� �� ��ȭ����")]
        public DialogEvent[] dialogs;
    }
}