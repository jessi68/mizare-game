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
        [FoldoutGroup("다이얼로그 UI", false)]
        [LabelText("다이얼로그 패널 UI"), Tooltip("다이얼로그 루트 게임오브젝트")]
        public GameObject dialogUI;
        [FoldoutGroup("다이얼로그 UI"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        [LabelText("양쪽 이미지"), Tooltip("다이얼로그 왼쪽/오른쪽 Image 컴포넌트")]
        public Image[] sideImg;
        [FoldoutGroup("다이얼로그 UI")]
        [LabelText("이름 TMP"), Tooltip("다이얼로그 창 이름 TMP")]
        public TMP_Text nameTMP;
        [FoldoutGroup("다이얼로그 UI")]
        [LabelText("스크립트 TMP"), Tooltip("다이얼로그 창 스크립트 TMP")]
        public CustomTMPEffect scriptTMP;
        [FoldoutGroup("다이얼로그 UI/캐릭터 떨림 효과", false)]
        [LabelText("강도"), Tooltip("캐릭터 떨림의 강도")]
        public float shakeIntensity;
        [FoldoutGroup("다이얼로그 UI/캐릭터 떨림 효과", false)]
        [LabelText("지속 시간"), Tooltip("캐릭터 떨림 효과 시간")]
        public float shakeTime;
        [FoldoutGroup("다이얼로그 UI/캐릭터 떨림 효과", false)]
        [LabelText("떨림 간격"), Tooltip("캐릭터가 얼만큼의 시간간격으로 떨릴지에 대한 수치")]
        public float shakeIntervalTime;
        [FoldoutGroup("다이얼로그 UI/캐릭터 바운스 효과", false)]
        [LabelText("지속 시간"), Tooltip("캐릭터가 뛰는 시간\n'1 / 지속시간'sec\nex)1 = 1sec, 2 = 0.5sec, 0.5 = 2sec")]
        public float bounceTime;
        [FoldoutGroup("다이얼로그 UI/캐릭터 바운스 효과", false)]
        [LabelText("높이"), Tooltip("캐릭터가 뛰어오르는 높이")]
        public float bounceHeight;

        // sideImg의 초기 위치 (FX초기화 할 때 사용)
        private Vector3[] sidePos;
        // sideFX 코루틴 정보 (스킵 시 사용)
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
            // 초기화를 위해 처음의 Left, Right 사이드 이미지의 위치 얻기
            for (int i = 0; i < 2; ++i)
                sidePos[i] = sideImg[i].transform.position;

            sideFXCoroutine = new Coroutine[2];
        }
        /// <summary>
        /// 다이얼로그 설정
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
        /// 다이얼로그 이벤트 발생시 호출되는 함수
        /// </summary>
        public void OnDialogEvent(DialogEvent de)
        {
            // 마우스 클릭시 타이핑이 안끝났다면 타이핑 끝내고, 타이핑이 다 되어있는 상태라면 다음 다이얼로그 설정
            if (!scriptTMP.IsDoneTyping)
                scriptTMP.SkipTyping();
            else
                gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        /// <summary>
        /// 캐릭터 이미지 설정
        /// </summary>
        /// <param name="side">왼쪽 사이드인지 오른쪽 사이드인지 설정</param>
        /// <param name="charImgIdx">보여줄 이미지</param>
        /// <param name="isHighlight">하이라이트 여부</param>
        /// <param name="charFX">사이드 이미지에 줄 효과</param>
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
        /// 효과 중간에 스킵될 수 있으므로, 효과들이 재생중이라면 종료시키고 이미지들의 위치 원상태로 복구하는 함수
        /// </summary>
        private void ResetEffects()
        {
            // 작동중인 효과 코루틴 멈추기
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
        [LabelText("선택지 내용"), Tooltip("선택지 내용")]
        public string str;
        [LabelText("선택 후 이동될 이벤트 번호"), Tooltip("해당 선택지 선택시 이동할 이벤트 번호")]
        public int nextIdx;
    }
    [System.Serializable]
    public struct ChoiceStruct
    {
        #region Fields
        [FoldoutGroup("선택지 UI", false)]
        [LabelText("선택지 패널 UI"), Tooltip("선택지 UI 루트 게임오브젝트")]
        public GameObject choiceUI;
        [FoldoutGroup("선택지 UI")]
        [LabelText("선택지 버튼들"), Tooltip("선택지 버튼들에 대한 RectTransform")]
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
            // 모든 선택지들 비활성화하고
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
        [FoldoutGroup("조사 UI", false)]
        [LabelText("조사 패널 UI"), Tooltip("조사 패널 루트 게임오브젝트")]
        public GameObject investigationPanel;
        [FoldoutGroup("조사 UI")]
        [LabelText("조사하는 캐릭터 UI"), Tooltip("좌상단에 있는 캐릭터 이미지 컴포넌트")]
        public Image investigationCharacter;
        [FoldoutGroup("조사 UI")]
        [LabelText("조사 완료 효과 애니메이터"), Tooltip("조사 완료 후 재생될 애니메이터")]
        public Animator findAllItemFXAnimator;
        [FoldoutGroup("조사 UI")]
        [LabelText("좌상단 다이얼로그"), Tooltip("조사를 완료하지 않거나 하는 등 좌상단 캐릭터의 대화내용을 표시하는 다이얼로그")]
        public CustomTMPEffect investigationDialogTMP;
        [FoldoutGroup("조사 UI/아이템 획득 창 UI", false)]
        [LabelText("아이템 획득 애니메이터"), Tooltip("아이템 획득 창에 대한 애니메이터")]
        public Animator getItemAnimator;
        [FoldoutGroup("조사 UI/아이템 획득 창 UI")]
        [LabelText("획득 아이템 이미지 UI"), Tooltip("아이템 획득 창에서의 이미지 컴포넌트")]
        public Image getItemUI_ItemImg;
        [FoldoutGroup("조사 UI/아이템 획득 창 UI")]
        [LabelText("획득 아이템 이름 UI"), Tooltip("아이템 획득 창에서의 이름 TMP")]
        public TMP_Text getItemUI_ItemName;
        [FoldoutGroup("조사 UI/아이템 획득 창 UI")]
        [LabelText("획득 아이템 설명 UI"), Tooltip("아이템 획득 창에서의 설명 TMP")]
        public TMP_Text getItemUI_ItemDesc;

        private GameObject investigationDialog;
        private Button clearBtn;
        /// <summary>
        /// 남은 아이템 개수
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
        /// 조사 모드 설정 함수
        /// </summary>
        /// <param name="ivChar">조사를 진행할 캐릭터 인덱스</param>
        /// <param name="nextIndex">조사가 끝난 후 이동할 다음 스크립트 인덱스</param>
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
        /// 조사 모드 해제 함수
        /// </summary>
        public void Release()
        {
            investigationPanel.SetActive(false);
        }
        public void OnUpdate()
        {
            if (gm.IsKeyDown())
            {
                // 화면에 아이템 획득 창이 떠있는가
                if (getItemAnimator.gameObject.activeInHierarchy)
                {
                    // 아이템 획득 창 애니메이션이 완료된 상태인가
                    if (getItemAnimator.GetCurrentAnimatorStateInfo(0).IsName("Complete"))
                    {
                        // 완료된 상태면 아이템 획득 창 비활성화 후 변수 초기화
                        getItemAnimator.gameObject.SetActive(false);
                        getItemAnimator.SetBool("Skip", false);
                    }
                    else
                        // 미완료된 상태면 스킵
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
                investigationDialogTMP.SetText("아직 조사가 끝나지 않았어");
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
        [LabelText("다이얼로그 UI"), Tooltip("다이얼로그 UI")]
        public GameObject dialogUI;
        [FoldoutGroup("정리 UI")]
        [LabelText("다이얼로그 화자 TMP"), Tooltip("다이얼로그에서 말하는 사람을 나타내는 TMP")]
        public GameObject nameTMP;
        [FoldoutGroup("정리 UI")]
        [LabelText("다이얼로그 대화내용 TMP"), Tooltip("다이얼로그에서 대화 내용을 나타내는 TMP")]
        public GameObject descTMP;
        [FoldoutGroup("정리 UI")]
        [LabelText("제출 버튼"), Tooltip("문장 완성 이벤트를 발생시키는 버튼")]
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