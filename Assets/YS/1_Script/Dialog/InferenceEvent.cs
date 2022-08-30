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
    [System.Serializable]
    public struct InferenceStruct
    {
        #region Fields
        [FoldoutGroup("�߸� UI", false)]
        [LabelText("�߸� �г� UI"), Tooltip("���� �г� ��Ʈ ���ӿ�����Ʈ")]
        public GameObject rootObj;
        [FoldoutGroup("�߸� UI")]
        [LabelText("�߸� ĳ���� �̹���"), Tooltip("�߸��ϴ� ĳ���� �̹��� ������Ʈ")]
        public Image character;
        [FoldoutGroup("�߸� UI")]
        [LabelText("�߸� ������ �̹���"), Tooltip("�߸��ϴ� ������ �̹��� ������Ʈ")]
        public Image item;
        [FoldoutGroup("�߸� UI")]
        [LabelText("������ ���� TMP"), Tooltip("�߸� ������ ���� TMP ������Ʈ")]
        public TMP_Text tmp_itemDesc;
        [FoldoutGroup("�߸� UI")]
        [LabelText("�߸� ��������"), Tooltip("�߸��� ���� �������� ��ư ������Ʈ��")]
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

            // �����̰ų� 2���� ��ȸ�� �� ���������
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
        /// ���̾�α� �̺�Ʈ �߻��� ȣ��Ǵ� �Լ�
        /// </summary>
        public void OnDialogEvent()
        {
            // ���콺 Ŭ���� Ÿ������ �ȳ����ٸ� Ÿ���� ������, Ÿ������ �� �Ǿ��ִ� ���¶�� ���� ���̾�α� ����
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