using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public enum ITEM_INDEX
    {
        RAT,
        EMPTY_BOTTLE,
        HAT,
        BLACK_CLOTH,
        CLOCK,
        MAX
    }
    public enum SIDE_IMAGE
    {
        LEFT_SIDE,
        RIGHT_SIDE
    }
    public enum SCREEN_EFFECT
    {
        NONE,
        FADE_IN,
        FADE_OUT,
        RED_FLASH,
    }
    public enum CHARACTER_IMAGE_INDEX
    {
        NONE,
        MIZAR,
        ALCOR,
        SENIOR,
        SCHOLAR,
        BLACKROBE,
        MAX
    }
    public enum CHARACTER_EFFECT_INDEX
    {
        NONE,
        SHAKE_VERTICAL,
        SHAKE_HORIZONTAL,
        SHAKE_RANDOM,
        BOUNCE
    }
    public struct SaveData
    {
        public int scriptIndex;
    }
    [System.Serializable]
    public struct DialogStruct
    {
        #region Fields
        [FoldoutGroup("���̾�α� UI", false)]
        [LabelText("���̾�α� �г� UI"), Tooltip("���̾�α� ��Ʈ ���ӿ�����Ʈ")]
        public GameObject dialogUI;
        [FoldoutGroup("���̾�α� UI"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        [LabelText("���� �̹���"), Tooltip("���̾�α� ����/������ Image ������Ʈ")]
        public Image[] sideImg;
        [FoldoutGroup("���̾�α� UI")]
        [LabelText("�̸� TMP"), Tooltip("���̾�α� â �̸� TMP")]
        public TMP_Text nameTMP;
        [FoldoutGroup("���̾�α� UI")]
        [LabelText("��ũ��Ʈ TMP"), Tooltip("���̾�α� â ��ũ��Ʈ TMP")]
        public CustomTMPEffect scriptTMP;
        [FoldoutGroup("���̾�α� UI/ĳ���� ���� ȿ��", false)]
        [LabelText("����"), Tooltip("ĳ���� ������ ����")]
        public float shakeIntensity;
        [FoldoutGroup("���̾�α� UI/ĳ���� ���� ȿ��", false)]
        [LabelText("���� �ð�"), Tooltip("ĳ���� ���� ȿ�� �ð�")]
        public float shakeTime;
        [FoldoutGroup("���̾�α� UI/ĳ���� ���� ȿ��", false)]
        [LabelText("���� ����"), Tooltip("ĳ���Ͱ� ��ŭ�� �ð��������� �������� ���� ��ġ")]
        public float shakeIntervalTime;
        [FoldoutGroup("���̾�α� UI/ĳ���� �ٿ ȿ��", false)]
        [LabelText("���� �ð�"), Tooltip("ĳ���Ͱ� �ٴ� �ð�\n'1 / ���ӽð�'sec\nex)1 = 1sec, 2 = 0.5sec, 0.5 = 2sec")]
        public float bounceTime;
        [FoldoutGroup("���̾�α� UI/ĳ���� �ٿ ȿ��", false)]
        [LabelText("����"), Tooltip("ĳ���Ͱ� �پ������ ����")]
        public float bounceHeight;

        // sideImg�� �ʱ� ��ġ (FX�ʱ�ȭ �� �� ���)
        private Vector3[] sidePos;
        // sideFX �ڷ�ƾ ���� (��ŵ �� ���)
        private Coroutine[] sideFXCoroutine;
        private GameManager gm;
        #endregion

        #region Properties
        public Vector3[] SidePosition => sidePos;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;

            sidePos = new Vector3[2];
            // �ʱ�ȭ�� ���� ó���� Left, Right ���̵� �̹����� ��ġ ���
            for (int i = 0; i < 2; ++i)
                sidePos[i] = sideImg[i].transform.position;

            sideFXCoroutine = new Coroutine[2];
        }
        /// <summary>
        /// ���̾�α� ����
        /// </summary>
        public void Setup(DialogEvent de)
        {
            dialogUI.SetActive(true);
            gm.ScreenEffect(de.ScreenEffect);

            if (de.Name == null || de.Name == "")
            {
                if (de.Script == null || de.Script == "")
                {
                    dialogUI.SetActive(false);
                    return;
                }

                nameTMP.transform.parent.gameObject.SetActive(false);
            }
            else
                nameTMP.transform.parent.gameObject.SetActive(true);

            gm.Logging($"<b>{de.Name}</b>\n<size=40>{de.Script}</size>\n");

            nameTMP.SetText(de.Name);
            scriptTMP.SetText(de.Script);

            SetCharSetting(SIDE_IMAGE.LEFT_SIDE, de.LeftImage, de.LeftHighlight, de.LeftEffect);
            SetCharSetting(SIDE_IMAGE.RIGHT_SIDE, de.RightImage, de.RightHighlight, de.RightEffect);
        }
        public void Release()
        {
            dialogUI.SetActive(false);
            ResetEffects();
        }
        /// <summary>
        /// ���̾�α� �̺�Ʈ �߻��� ȣ��Ǵ� �Լ�
        /// </summary>
        public void OnDialogEvent(DialogEvent de)
        {
            // ���콺 Ŭ���� Ÿ������ �ȳ����ٸ� Ÿ���� ������, Ÿ������ �� �Ǿ��ִ� ���¶�� ���� ���̾�α� ����
            if (!scriptTMP.IsDoneTyping)
                scriptTMP.SkipTyping();
            else
                gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        /// <summary>
        /// ĳ���� �̹��� ����
        /// </summary>
        /// <param name="side">���� ���̵����� ������ ���̵����� ����</param>
        /// <param name="charImgIdx">������ �̹���</param>
        /// <param name="isHighlight">���̶���Ʈ ����</param>
        /// <param name="charFX">���̵� �̹����� �� ȿ��</param>
        private void SetCharSetting(SIDE_IMAGE side, CHARACTER_IMAGE_INDEX charImgIdx, bool isHighlight, CHARACTER_EFFECT_INDEX charFX)
        {
            sideImg[(int)side].sprite = gm.charImgs[(int)charImgIdx];

            if (charImgIdx == CHARACTER_IMAGE_INDEX.NONE)
                sideImg[(int)side].color = Color.clear;
            else
            {
                sideImg[(int)side].color = isHighlight ? Color.white : Color.gray;
                sideImg[(int)side].SetNativeSize();
            }

            switch (charFX)
            {
                case CHARACTER_EFFECT_INDEX.SHAKE_HORIZONTAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_VERTICAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_RANDOM:
                    sideFXCoroutine[(int)side] = gm.StartCoroutine(gm.ShakeEffect(sideImg[(int)side].gameObject.transform, shakeIntensity, shakeTime, shakeIntervalTime, charFX));
                    break;

                case CHARACTER_EFFECT_INDEX.BOUNCE:
                    sideFXCoroutine[(int)side] = gm.StartCoroutine(gm.BounceEffect(sideImg[(int)side].gameObject.transform, bounceTime, bounceHeight));
                    break;
            }
        }
        /// <summary>
        /// ȿ�� �߰��� ��ŵ�� �� �����Ƿ�, ȿ������ ������̶�� �����Ű�� �̹������� ��ġ �����·� �����ϴ� �Լ�
        /// </summary>
        private void ResetEffects()
        {
            // �۵����� ȿ�� �ڷ�ƾ ���߱�
            if (sideFXCoroutine[0] != null) gm.StopCoroutine(sideFXCoroutine[0]);
            if (sideFXCoroutine[1] != null) gm.StopCoroutine(sideFXCoroutine[1]);
            if (gm.bgFXCoroutine != null) gm.StopCoroutine(gm.bgFXCoroutine);

            for (int i = 0; i < 2; ++i)
                sideImg[i].transform.position = SidePosition[i];

            gm.SetBGCurTime(0.0f);
            gm.SetBGFadeInOut(true);
            gm.ResetFlash();
        }
        #endregion
    }
    [System.Serializable]
    public struct ChoiceData
    {
        [LabelText("������ ����"), Tooltip("������ ����")]
        public string str;
        [LabelText("���� �� �̵��� �̺�Ʈ ��ȣ"), Tooltip("�ش� ������ ���ý� �̵��� �̺�Ʈ ��ȣ")]
        public int nextIdx;
    }
    [System.Serializable]
    public struct ChoiceStruct
    {
        #region Fields
        [FoldoutGroup("������ UI", false)]
        [LabelText("������ �г� UI"), Tooltip("������ UI ��Ʈ ���ӿ�����Ʈ")]
        public GameObject choiceUI;
        [FoldoutGroup("������ UI")]
        [LabelText("������ ��ư��"), Tooltip("������ ��ư�鿡 ���� RectTransform")]
        public RectTransform[] choiceBtns;

        private TMP_Text[] choiceTMPs;
        private ChoiceData[] choices;
        #endregion

        #region Methods
        public void Initialize()
        {
            choiceTMPs = new TMP_Text[choiceBtns.Length];
            for (int i = 0; i < choiceBtns.Length; ++i)
            {
                int index = i;
                choiceTMPs[i] = choiceBtns[i].GetChild(0).GetComponent<TMP_Text>();
                choiceBtns[i].GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.choiceStruct.OnChooseChoice(index); });
            }
        }
        public void Setup(ChoiceData[] choices)
        {
            choiceUI.SetActive(true);
            this.choices = choices;
            SetChoice();
        }
        private void SetChoice()
        {
            float padding = (1 - (choices.Length * 0.15f)) / (choices.Length + 1);
            float height = 1.0f;
            for (int i = 0; i < choices.Length; ++i)
            {
                choiceTMPs[i].SetText(choices[i].str);
                choiceBtns[i].gameObject.SetActive(true);
                height -= padding;
                choiceBtns[i].anchorMax = new Vector2(1.0f, height);
                height -= 0.15f;
                choiceBtns[i].anchorMin = new Vector2(0.0f, height);
            }
        }
        public void OnChooseChoice(int index)
        {
            // ��� �������� ��Ȱ��ȭ�ϰ�
            for (int i = 0; i < choices.Length; ++i)
                choiceBtns[i].gameObject.SetActive(false);

            GameManager.Instance.scriptData.SetScript(choices[index].nextIdx);
        }
        public void Release()
        {
            choiceUI.SetActive(false);
        }
        #endregion
    }
    [System.Serializable]
    public struct InvestigationStruct
    {
        #region Fields
        [FoldoutGroup("���� UI", false)]
        [LabelText("���� �г� UI"), Tooltip("���� �г� ��Ʈ ���ӿ�����Ʈ")]
        public GameObject investigationPanel;
        [FoldoutGroup("���� UI")]
        [LabelText("�����ϴ� ĳ���� UI"), Tooltip("�»�ܿ� �ִ� ĳ���� �̹��� ������Ʈ")]
        public Image investigationCharacter;
        [FoldoutGroup("���� UI")]
        [LabelText("���� �Ϸ� ȿ�� �ִϸ�����"), Tooltip("���� �Ϸ� �� ����� �ִϸ�����")]
        public Animator findAllItemFXAnimator;
        [FoldoutGroup("���� UI")]
        [LabelText("�»�� ���̾�α�"), Tooltip("���縦 �Ϸ����� �ʰų� �ϴ� �� �»�� ĳ������ ��ȭ������ ǥ���ϴ� ���̾�α�")]
        public CustomTMPEffect investigationDialogTMP;
        [FoldoutGroup("���� UI/������ ȹ�� â UI", false)]
        [LabelText("������ ȹ�� �ִϸ�����"), Tooltip("������ ȹ�� â�� ���� �ִϸ�����")]
        public Animator getItemAnimator;
        [FoldoutGroup("���� UI/������ ȹ�� â UI")]
        [LabelText("ȹ�� ������ �̹��� UI"), Tooltip("������ ȹ�� â������ �̹��� ������Ʈ")]
        public Image getItemUI_ItemImg;
        [FoldoutGroup("���� UI/������ ȹ�� â UI")]
        [LabelText("ȹ�� ������ �̸� UI"), Tooltip("������ ȹ�� â������ �̸� TMP")]
        public TMP_Text getItemUI_ItemName;
        [FoldoutGroup("���� UI/������ ȹ�� â UI")]
        [LabelText("ȹ�� ������ ���� UI"), Tooltip("������ ȹ�� â������ ���� TMP")]
        public TMP_Text getItemUI_ItemDesc;

        private GameObject investigationDialog;
        private Button clearBtn;
        /// <summary>
        /// ���� ������ ����
        /// </summary>
        private int findCount;
        private GameManager gm;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;
            investigationCharacter.GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.ivStruct.OnClearBtnDown(); });
            investigationDialog = investigationDialogTMP.transform.parent.gameObject;
        }

        /// <summary>
        /// ���� ��� ���� �Լ�
        /// </summary>
        /// <param name="ivChar">���縦 ������ ĳ���� �ε���</param>
        /// <param name="nextIndex">���簡 ���� �� �̵��� ���� ��ũ��Ʈ �ε���</param>
        public void Setup(CHARACTER_IMAGE_INDEX ivChar)
        {
            investigationCharacter.sprite = gm.charImgs[(int)ivChar];

            investigationPanel.SetActive(true);
            
            var items = new Item[gm.ItemCount];
            for (int i = 0; i < items.Length; ++i)
            {
                items[i] = gm.GetItem(i);
                items[i].imageComp.raycastTarget = true;
            }

            findCount = items.Length;
            if (findCount == 0)
                findAllItemFXAnimator.SetBool("IsFindAllItem", true);
        }
        /// <summary>
        /// ���� ��� ���� �Լ�
        /// </summary>
        public void Release()
        {
            investigationPanel.SetActive(false);
        }
        public void OnUpdate()
        {
            if (gm.IsKeyDown())
            {
                // ȭ�鿡 ������ ȹ�� â�� ���ִ°�
                if (getItemAnimator.gameObject.activeInHierarchy)
                {
                    // ������ ȹ�� â �ִϸ��̼��� �Ϸ�� �����ΰ�
                    if (getItemAnimator.GetCurrentAnimatorStateInfo(0).IsName("Complete"))
                    {
                        // �Ϸ�� ���¸� ������ ȹ�� â ��Ȱ��ȭ �� ���� �ʱ�ȭ
                        getItemAnimator.gameObject.SetActive(false);
                        getItemAnimator.SetBool("Skip", false);
                    }
                    else
                        // �̿Ϸ�� ���¸� ��ŵ
                        getItemAnimator.SetBool("Skip", true);
                }
            }
        }
        public void OnFindItem(Item item)
        {
            item.gameObject.SetActive(false);
            --findCount;
            gm.invenComp.AddItem(item.index);

            getItemAnimator.gameObject.SetActive(true);
            getItemUI_ItemImg.sprite = item.ItemImage;
            getItemUI_ItemName.text = item.Name;
            getItemUI_ItemDesc.text = item.Desc;

            if (findCount == 0)
                findAllItemFXAnimator.SetBool("IsFindAllItem", true);
        }
        private void OnClearBtnDown()
        {
            if (findCount == 0)
                gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
            else
            {
                investigationDialog.SetActive(true);
                gm.CancelInvoke(nameof(HideInferenceDialogTMP));
                investigationDialogTMP.SetText("���� ���簡 ������ �ʾҾ�");
                gm.Invoke(nameof(HideInferenceDialogTMP), 3.0f);
            }
        }
        private void HideInferenceDialogTMP()
        {
            investigationDialog.SetActive(false);
        }
        #endregion
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
            character.sprite = gm.charImgs[(int)ie.CharacterIndex];
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

                if (bExit)  gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
                else        rootObj.SetActive(true);
            }
            else
                gm.dialogStruct.Setup(choiceDatas[lastChoiceIndex].dialogs[curDialogIndex]);
        }
        #endregion
    }
    [System.Serializable]
    public struct ArrangeStruct
    {
        #region Fields
        [FoldoutGroup("���� UI", false)]
        [LabelText("���� �г� UI"), Tooltip("���� �г� ��Ʈ ���ӿ�����Ʈ")]
        public GameObject rootObj;
        [FoldoutGroup("���� UI")]
        [LabelText("���� TMP"), Tooltip("������ ���� ������ ������ TMP")]
        public TMP_Text questionTMP;
        [FoldoutGroup("���� UI")]
        [LabelText("�ܾ� ������Ʈ"), Tooltip("���� �г��� �ܾ���� ������Ʈ")]
        public WordComponent[] words;
        [FoldoutGroup("���� UI")]
        [LabelText("���̾�α� UI"), Tooltip("���̾�α� UI")]
        public GameObject dialogUI;
        [FoldoutGroup("���� UI")]
        [LabelText("���̾�α� ȭ�� TMP"), Tooltip("���̾�α׿��� ���ϴ� ����� ��Ÿ���� TMP")]
        public GameObject nameTMP;
        [FoldoutGroup("���� UI")]
        [LabelText("���̾�α� ��ȭ���� TMP"), Tooltip("���̾�α׿��� ��ȭ ������ ��Ÿ���� TMP")]
        public GameObject descTMP;
        [FoldoutGroup("���� UI")]
        [LabelText("���� ��ư"), Tooltip("���� �ϼ� �̺�Ʈ�� �߻���Ű�� ��ư")]
        public Button submitBtn;

        private ArrangeEvent.Word[] wordsData;
        #endregion

        #region Methods
        public void Initialize()
        {
            submitBtn.onClick.AddListener(Submit);
        }
        public void Setup(ArrangeEvent ae)
        {
            rootObj.SetActive(true);

            questionTMP.text = ae.Question;

            for (int i = 0; i < 4; ++i)
                words[i].SetSetting(ae.Words[i]);

            wordsData = ae.Words;
        }
        public void OnUpdate()
        {
        }
        public void Release()
        {
            rootObj.SetActive(false);
        }
        private void Submit()
        {
            for (int i = 0; i < 4; ++i)
            {
                if (wordsData[i].correctIndex != words[i].Index)
                {
                    Fail();
                    return;
                }
            }
            Success();
        }
        private void Fail()
        {

        }
        private void Success()
        {

        }
        #endregion
    }
    public class ResourceManager
    {
        static Dictionary<string, Object> resourceMap = new Dictionary<string, Object>();

        public static T GetResource<T>(string path) where T : Object
        {
            if (resourceMap.ContainsKey(path))
                return (T)resourceMap[path];

            T obj = Resources.Load<T>(path);
            resourceMap.Add(path, obj);
            return obj;
        }

        public static void Remove(string path)
        {
            Resources.UnloadAsset(resourceMap[path]);
            resourceMap.Remove(path);
        }

        public static void Clear()
        {
            foreach (Object o in resourceMap.Values)
                Resources.UnloadAsset(o);
            resourceMap.Clear();
        }
    }
    public struct CachedWaitForSeconds
    {
        private static Dictionary<float, WaitForSeconds> cache = new Dictionary<float, WaitForSeconds>();
        public static WaitForSeconds Get(float time)
        {
            if (!cache.ContainsKey(time))
                cache.Add(time, new WaitForSeconds(time));

            return cache[time];
        }
    }
    [System.Serializable]
    public struct Bezier
    {
        public Vector3[] bezierPos;

        public Vector3 GetBezierPosition(float t)
        {
            Vector3[] result = (Vector3[])bezierPos.Clone();

            for (int i = result.Length - 1; i > 0; --i)
                for (int j = 0; j < i; ++j)
                    result[j] = Vector3.Lerp(result[j], result[j + 1], t);

            return result[0];
        }
    }
}