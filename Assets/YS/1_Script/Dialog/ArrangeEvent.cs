using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class ArrangeEvent : BaseScriptEvent
    {
        #region Fields
        [SerializeField]
        [LabelText("���� ����")]
        private string question;
        [SerializeField]
        [LabelText("�ܾ�"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true), DisableContextMenu]
        private Word[] words = new Word[4];
        [SerializeField, TextArea]
        [LabelText("��Ʈ")]
        private string hintStr;
        [SerializeField, TextArea]
        [LabelText("���� �� ����")]
        private string successStr;
        [SerializeField, TextArea]
        [LabelText("���� �� ����")]
        private string failStr;
        #endregion

        #region Properties
        public string Question => question;
        public Word[] Words => words;
        public string HintStr => hintStr;
        public string SuccessStr => successStr;
        public string FailStr => failStr;
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();

            gm.arStruct.Setup(this);
        }
        protected override void OnUpdate()
        {
            gm.arStruct.OnUpdate();
        }
        public override void OnExit()
        {
            gm.arStruct.Release();

            base.OnExit();
        }

        [System.Serializable, DisableContextMenu]
        public struct Word
        {
            [LabelText("�����ܾ��ΰ�?")]
            public bool isFixedWord;
            [ShowIf("isFixedWord")]
            [LabelText("�����ܾ� ���ڿ�")]
            public string fixedWord;
            [HideIf("isFixedWord")]
            [LabelText("���ôܾ� ���ڿ���")]
            public string[] choiceWords;
            [SerializeField, HideIf("isFixedWord"), MinValue("@choiceWords.Length == 0 ? 0 : 1"), MaxValue("@choiceWords.Length")]
            [LabelText("����"), Tooltip("�߸� ���������� �ùٸ� ��")]
            public int correctIndex;
        }
    }
}