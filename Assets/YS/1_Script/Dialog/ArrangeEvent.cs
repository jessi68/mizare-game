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
        [LabelText("단어"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true), DisableContextMenu]
        private Word[] words = new Word[4];
        [SerializeField, TextArea]
        [LabelText("힌트")]
        private string hintStr;
        [SerializeField, TextArea]
        [LabelText("성공 시 내용")]
        private string successStr;
        [SerializeField, TextArea]
        [LabelText("실패 시 내용")]
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
            [LabelText("고정단어인가?")]
            public bool isFixedWord;
            [ShowIf("isFixedWord")]
            [LabelText("고정단어 문자열")]
            public string fixedWord;
            [HideIf("isFixedWord")]
            [LabelText("선택단어 문자열들")]
            public string[] choiceWords;
            [SerializeField, HideIf("isFixedWord"), MinValue("@choiceWords.Length == 0 ? 0 : 1"), MaxValue("@choiceWords.Length")]
            [LabelText("정답"), Tooltip("추리 선택지들중 올바른 답")]
            public int correctIndex;
        }
    }
}