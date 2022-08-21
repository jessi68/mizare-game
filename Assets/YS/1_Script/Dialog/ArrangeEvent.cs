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
        [LabelText("�ܾ�")]
        private Word[] words = new Word[4];
        #endregion

        #region Properties
        public string Question => question;
        public Word[] Words => words;
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();

            gm.arStruct.Setup(this);
        }
        protected override void OnUpdate()
        {
        }
        public override void OnExit()
        {
            gm.arStruct.Release();

            base.OnExit();
        }

        [System.Serializable]
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
            [SerializeField, MaxValue("@choiceWords.Length - 1")]
            [LabelText("����"), Tooltip("�߸� ���������� �ùٸ� ��")]
            public int correctIndex;
        }
    }
}