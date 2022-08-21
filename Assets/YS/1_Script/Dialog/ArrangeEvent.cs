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
        [LabelText("질문 내용")]
        private string question;
        [SerializeField]
        [LabelText("단어")]
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
            [LabelText("고정단어인가?")]
            public bool isFixedWord;
            [ShowIf("isFixedWord")]
            [LabelText("고정단어 문자열")]
            public string fixedWord;
            [HideIf("isFixedWord")]
            [LabelText("선택단어 문자열들")]
            public string[] choiceWords;
            [SerializeField, MaxValue("@choiceWords.Length - 1")]
            [LabelText("정답"), Tooltip("추리 선택지들중 올바른 답")]
            public int correctIndex;
        }
    }
}