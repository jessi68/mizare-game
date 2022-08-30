using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    [System.Serializable]
    public struct ArrangeStruct
    {
        #region Fields
        [FoldoutGroup("정리 UI", false)]
        [LabelText("정리 패널 UI"), Tooltip("정리 패널 루트 게임오브젝트")]
        public GameObject rootObj;
        [FoldoutGroup("정리 UI")]
        [LabelText("질문 TMP"), Tooltip("질문에 대한 내용이 보여질 TMP")]
        public TMP_Text questionTMP;
        [FoldoutGroup("정리 UI")]
        [LabelText("단어 컴포넌트"), Tooltip("정리 패널의 단어들의 컴포넌트")]
        public WordComponent[] words;
        [FoldoutGroup("정리 UI")]
        [LabelText("미자르 이미지 컴포넌트"), Tooltip("미자르의 이미지 컴포넌트")]
        public Image mizarImg;
        [FoldoutGroup("정리 UI")]
        [LabelText("다이얼로그 대화내용 TMP"), Tooltip("다이얼로그에서 대화 내용을 나타내는 TMP")]
        public TMP_Text descTMP;
        [FoldoutGroup("정리 UI")]
        [LabelText("제출 버튼"), Tooltip("문장 완성 이벤트를 발생시키는 버튼")]
        public Button submitBtn;

        private ArrangeEvent.Word[] wordsData;
        private string successStr, failStr;
        private bool isSubmit;
        private GameManager gm;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;

            submitBtn.onClick.AddListener(Submit);
        }
        public void Setup(ArrangeEvent ae)
        {
            rootObj.SetActive(true);

            questionTMP.text = ae.Question;
            descTMP.text = ae.HintStr;
            mizarImg.sprite = ResourceManager.GetResource<Sprite>(ImageReference.Mizar_Normal);

            for (int i = 0; i < 4; ++i)
                words[i].SetSetting(ae.Words[i]);

            wordsData = ae.Words;
            successStr = ae.SuccessStr;
            failStr = ae.FailStr;

            isSubmit = false;
        }
        public void OnUpdate()
        {
            if (isSubmit && gm.IsKeyDown())
                gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        public void Release()
        {
            rootObj.SetActive(false);
        }
        private void Submit()
        {
            gm.arStruct.isSubmit = true;
            for (int i = 0; i < 4; ++i)
            {
                if (!gm.arStruct.wordsData[i].isFixedWord && gm.arStruct.wordsData[i].correctIndex != gm.arStruct.words[i].Index)
                {
                    mizarImg.sprite = ResourceManager.GetResource<Sprite>(ImageReference.Mizar_Normal);
                    gm.arStruct.descTMP.text = gm.arStruct.failStr;
                    return;
                }
            }
            mizarImg.sprite = ResourceManager.GetResource<Sprite>(ImageReference.Mizar_Normal);
            gm.arStruct.descTMP.text = gm.arStruct.successStr;
        }
        #endregion
    }
}