using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class InferenceEvent : BaseScriptEvent
    {
        #region Fields
        [SerializeField]
        [LabelText("추리를 진행할 캐릭터")]
        private Sprite charIndex;
        [SerializeField]
        [LabelText("추리할 아이템")]
        private ITEM_INDEX itemIndex;
        [SerializeField]
        [LabelText("선택지들")]
        private InferenceDialogData[] choiceDatas;
        [BoxGroup("틀렸던 선택지 선택시 나오는 문구", true, true), SerializeField]
        [HideLabel]
        private DialogEvent twiceFailDialogData;
        [SerializeField, MaxValue("@choiceDatas.Length - 1")]
        [LabelText("정답"), Tooltip("추리 선택지들중 올바른 답")]
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
        [LabelText("선택지 내용")]
        public string choiceStr;
        [LabelText("선택 후 대화내용")]
        public DialogEvent[] dialogs;
    }
    [System.Serializable]
    public struct InferenceStruct
    {
        #region Fields
        [FoldoutGroup("추리 UI", false)]
        [LabelText("추리 패널 UI"), Tooltip("조사 패널 루트 게임오브젝트")]
        public GameObject rootObj;
        [FoldoutGroup("추리 UI")]
        [LabelText("추리 캐릭터 이미지"), Tooltip("추리하는 캐릭터 이미지 컴포넌트")]
        public Image character;
        [FoldoutGroup("추리 UI")]
        [LabelText("추리 아이템 이미지"), Tooltip("추리하는 아이템 이미지 컴포넌트")]
        public Image item;
        [FoldoutGroup("추리 UI")]
        [LabelText("아이템 설명 TMP"), Tooltip("추리 아이템 설명 TMP 컴포넌트")]
        public TMP_Text tmp_itemDesc;
        [FoldoutGroup("추리 UI")]
        [LabelText("추리 선택지들"), Tooltip("추리에 대한 선택지의 버튼 컴포넌트들")]
        public Button[] choiceBtns;

        private TMP_Text[] choiceTMPs;
        private InferenceDialogData[] choiceDatas;
        private DialogEvent twiceFailDialogData;
        private uint correctIndex;
        private int lastChoiceIndex;
        private int curDialogIndex;
        private bool bExit;
        private GameManager gm;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;
            choiceTMPs = new TMP_Text[choiceBtns.Length];
            for (uint i = 0; i < choiceBtns.Length; ++i)
            {
                uint index = i;
                choiceTMPs[i] = choiceBtns[i].transform.GetChild(0).GetComponent<TMP_Text>();
                choiceBtns[i].onClick.AddListener(() => { GameManager.Instance.ifStruct.Choice(index); });
            }
        }
        public void Setup(InferenceEvent ie)
        {
            rootObj.SetActive(true);
            character.sprite = ie.CharacterIndex;
            item.sprite = gm.itemData[ie.ItemIndex].img;
            tmp_itemDesc.text = gm.itemData[ie.ItemIndex].desc;

            choiceDatas = ie.ChoiceDatas;
            for (int i = 0; i < choiceDatas.Length; ++i)
            {
                choiceTMPs[i].text = choiceDatas[i].choiceStr;
                choiceBtns[i].gameObject.SetActive(true);
            }
            twiceFailDialogData = ie.TwiceFailDialogData;
            correctIndex = ie.CorrectIndex;
            lastChoiceIndex = -1;
            curDialogIndex = 0;
            bExit = false;
        }
        public void OnUpdate()
        {
            if (!rootObj.activeInHierarchy && gm.IsKeyDown())
                OnDialogEvent();
        }
        public void Release()
        {
            rootObj.SetActive(false);
            for (int i = 0; i < choiceDatas.Length; ++i)
                choiceBtns[i].gameObject.SetActive(false);
        }
        private void Choice(uint choiceIndex)
        {
            curDialogIndex = 0;
            rootObj.SetActive(false);

            // 정답이거나 2번의 기회를 다 사용헀는지
            if (choiceIndex == correctIndex || lastChoiceIndex != -1)
            {
                bExit = true;
                if (lastChoiceIndex == choiceIndex)
                {
                    gm.dialogStruct.Setup(twiceFailDialogData);
                    return;
                }
            }

            lastChoiceIndex = (int)choiceIndex;

            gm.dialogStruct.Setup(choiceDatas[lastChoiceIndex].dialogs[curDialogIndex]);
        }
        /// <summary>
        /// 다이얼로그 이벤트 발생시 호출되는 함수
        /// </summary>
        public void OnDialogEvent()
        {
            // 마우스 클릭시 타이핑이 안끝났다면 타이핑 끝내고, 타이핑이 다 되어있는 상태라면 다음 다이얼로그 설정
            if (!gm.dialogStruct.scriptTMP.IsDoneTyping)
                gm.dialogStruct.scriptTMP.SkipTyping();
            else if (++curDialogIndex == choiceDatas[lastChoiceIndex].dialogs.Length)
            {
                gm.dialogStruct.Release();

                if (bExit) gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
                else rootObj.SetActive(true);
            }
            else
                gm.dialogStruct.Setup(choiceDatas[lastChoiceIndex].dialogs[curDialogIndex]);
        }
        #endregion
    }
}