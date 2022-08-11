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
        [LabelText("추리를 진행할 캐릭터")]
        private CHARACTER_IMAGE_INDEX charIndex;
        [SerializeField]
        [LabelText("추리할 아이템")]
        private ITEM_INDEX itemIndex;
        [SerializeField]
        [LabelText("선택지들")]
        private InferenceDialogData[] choiceDatas;
        [BoxGroup("틀렸던 선택지 선택시 나오는 문구", true, true), SerializeField]
        [HideLabel]
        private DialogEvent twiceFailDialogData = new DialogEvent(false);
        [SerializeField, MaxValue("@choiceDatas.Length - 1")]
        [LabelText("정답"), Tooltip("추리 선택지들중 올바른 답")]
        private uint correctIndex;
        [SerializeField]
        [LabelText("추리 이벤트가 끝난 후 이동할 이벤트 번호")]
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
        [LabelText("선택지 내용")]
        public string choiceStr;
        [ListDrawerSettings(CustomAddFunction = nameof(AddFunction))]
        [LabelText("선택 후 대화내용")]
        public DialogEvent[] dialogs;

        private DialogEvent AddFunction()
        {
            return new DialogEvent(false);
        }
    }
}