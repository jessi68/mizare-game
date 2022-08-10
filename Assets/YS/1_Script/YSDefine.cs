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
        [FoldoutGroup("���̾�α� UI"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true), OnInspectorInit(nameof(OnInspectorInitFunc))]
        [LabelText("���� �̹���"), Tooltip("���̾�α� ����/������ Image ������Ʈ")]
        public Image[] sideImg;
        [FoldoutGroup("���̾�α� UI"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true), OnInspectorInit(nameof(OnInspectorInitFunc))]
        [LabelText("�̸� TMP"), Tooltip("���̾�α� â �̸� TMP")]
        public TMP_Text nameTMP;
        [FoldoutGroup("���̾�α� UI"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true), OnInspectorInit(nameof(OnInspectorInitFunc))]
        [LabelText("��ũ��Ʈ TMP"), Tooltip("���̾�α� â ��ũ��Ʈ TMP")]
        public CustomTMPEffect scriptTMP;

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
        public void Setup(SCREEN_EFFECT se, string name, string script, CHARACTER_IMAGE_INDEX leftImg, bool leftHighlight, CHARACTER_EFFECT_INDEX leftFX, CHARACTER_IMAGE_INDEX rightImg, bool rightHighlight, CHARACTER_EFFECT_INDEX rightFX)
        {
            dialogUI.SetActive(true);
            gm.ScreenEffect(se);

            if (name == null || name == "")
            {
                if (script == null || script == "")
                {
                    dialogUI.SetActive(false);
                    return;
                }

                nameTMP.transform.parent.gameObject.SetActive(false);
            }
            else
                nameTMP.transform.parent.gameObject.SetActive(true);

            gm.Logging($"<b>{name}</b>\n<size=40>{script}</size>\n");

            nameTMP.SetText(name);
            scriptTMP.SetText(script);

            SetCharSetting(SIDE_IMAGE.LEFT_SIDE, leftImg, leftHighlight, leftFX);
            SetCharSetting(SIDE_IMAGE.RIGHT_SIDE, rightImg, rightHighlight, rightFX);
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
                gm.scriptData.SetScript(de.NextIdx);
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
                    sideFXCoroutine[(int)side] = gm.StartCoroutine(gm.ShakeEffect(sideImg[(int)side].gameObject.transform, 5, 0.5f, 0.01f, charFX));
                    break;

                case CHARACTER_EFFECT_INDEX.BOUNCE:
                    sideFXCoroutine[(int)side] = gm.StartCoroutine(gm.BounceEffect(sideImg[(int)side].gameObject.transform, 3.0f));
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

            gm.SetBGCurTime(1.0f);
            gm.SetBGFadeInOut(false);
            gm.ResetFlash();
        }
        private void OnInspectorInitFunc()
        {
            sideImg = new Image[2];
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
            var cd = this;
            choiceTMPs = new TMP_Text[choiceBtns.Length];
            for (int i = 0; i < choiceBtns.Length; ++i)
            {
                choiceTMPs[i] = choiceBtns[i].GetChild(0).GetComponent<TMP_Text>();
                choiceBtns[i].GetComponent<Button>().onClick.AddListener(() => { cd.OnChooseChoice(i); });
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
        
        /// <summary>
        /// ���� ������ ����
        /// </summary>
        private int findCount;
        /// <summary>
        /// �̺�Ʈ ���� �� �̵��� �ε���
        /// </summary>
        private int nextIndex;
        private GameManager gm;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;
        }

        /// <summary>
        /// ���� ��� ���� �Լ�
        /// </summary>
        /// <param name="ivChar">���縦 ������ ĳ���� �ε���</param>
        /// <param name="nextIndex">���簡 ���� �� �̵��� ���� ��ũ��Ʈ �ε���</param>
        public void Setup(CHARACTER_IMAGE_INDEX ivChar, int nextIndex)
        {
            investigationCharacter.sprite = gm.charImgs[(int)ivChar];
            this.nextIndex = nextIndex;

            investigationPanel.SetActive(true);
            
            var items = new Item[gm.ItemCount];
            for (int i = 0; i < items.Length; ++i)
            {
                items[i] = gm.GetItem(i);
                items[i].imageComp.raycastTarget = true;
            }

            findCount = items.Length;

            gm.SetBGCurTime(0.0f);
        }
        /// <summary>
        /// ���� ��� ���� �Լ�
        /// </summary>
        public void Release()
        {
            investigationPanel.SetActive(false);
            gm.scriptData.SetScript(nextIndex);
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
        #endregion
    }
    public struct InferenceStruct
    {
        #region Fields
        public GameObject rootObj;
        public Image character;
        public Image item;
        public TMP_Text tmp_itemDesc;
        public Button[] choiceBtns;
        public uint correctIndex;
        public int nextIndex;

        private TMP_Text[] choiceTMPs;
        private InferenceDialogData[] choiceDatas;
        private string twiceFailStr;
        private int lastChoiceIndex;
        private bool bExit;
        private GameManager gm;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;

            var id = this;
            for (uint i = 0; i < choiceBtns.Length; ++i)
            {
                choiceTMPs[i] = choiceBtns[i].transform.GetChild(0).GetComponent<TMP_Text>();
                choiceBtns[i].onClick.AddListener(() => { id.Choice(i); });
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
                choiceTMPs[i].text = choiceDatas[i].choiceStr;

            lastChoiceIndex = -1;
            bExit = false;
        }
        public void Release()
        {
            rootObj.SetActive(false);
        }
        private void Choice(uint choiceIndex)
        {
            // �ùٸ� ���ý�
            if (correctIndex == choiceIndex)
            {
                bExit = true;
            }
            // Ʋ�ȴ� ������ �� ���ý�
            else if (lastChoiceIndex == choiceIndex)
            {
                bExit = true;
            }
            // Ʋ�� ���ý�
            else if (lastChoiceIndex == -1)
                lastChoiceIndex = (int)choiceIndex;
            else
                bExit = true;

            //gm.dialogStruct.Setup(SCREEN_EFFECT.NONE, choiceDatas[choiceIndex].dialogs);
        }
        private void SetDialog(uint dialogIndex)
        {

        }
        /// <summary>
        /// ���̾�α� �̺�Ʈ �߻��� ȣ��Ǵ� �Լ�
        /// </summary>
        public void OnDialogEvent(DialogEvent de)
        {
            //// ���콺 Ŭ���� Ÿ������ �ȳ����ٸ� Ÿ���� ������, Ÿ������ �� �Ǿ��ִ� ���¶�� ���� ���̾�α� ����
            //if (!scriptTMP.IsDoneTyping)
            //    scriptTMP.SkipTyping();
            //else
            //    gm.scriptData.SetScript(de.NextIdx);
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