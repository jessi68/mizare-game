using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        public GameObject dialogUI;

        public Image[] sideImg;

        public TMP_Text nameTMP;
        public CustomTMPEffect scriptTMP;

        // sideImg의 초기 위치 (FX초기화 할 때 사용)
        private Vector3[] sidePos;

        public Vector3[] SidePosition => sidePos;

        public void Initialize()
        {
            sidePos = new Vector3[2];
            // 초기화를 위해 처음의 Left, Right 사이드 이미지의 위치 얻기
            for (int i = 0; i < 2; ++i)
                sidePos[i] = sideImg[i].transform.position;
        }
    }
    [System.Serializable]
    public struct ChoiceData
    {
        [Tooltip("선택지 내용")]
        public string str;
        [Tooltip("해당 선택지 선택시 이동할 이벤트 번호")]
        public uint nextIdx;
    }
    [System.Serializable]
    public struct ChoiceStruct
    {
        public GameObject choiceUI;
        public RectTransform[] choiceBtns;
        [HideInInspector]
        public TMP_Text[] choiceTMPs;

        public void Initialize()
        {
            choiceTMPs = new TMP_Text[choiceBtns.Length];
            for (int i = 0; i < choiceBtns.Length; ++i)
                choiceTMPs[i] = choiceBtns[i].GetChild(0).GetComponent<TMP_Text>();
        }

        public void SetChoice(ChoiceData[] choices)
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
    }
    [System.Serializable]
    public struct InvestigationStruct
    {
        public GameObject investigationUI;

        public GameObject investigationPanel;
        public Image investigationCharacter;
        public Animator findAllItemFXAnimator;
        public GameObject investigationDialog;
        public Animator getItemAnimator;
        public Image getItemUI_ItemImg;
        public TMP_Text getItemUI_ItemName;
        public TMP_Text getItemUI_ItemDesc;
        [HideInInspector]
        public CustomTMPEffect investigationDialogTMP;
        
        public GameObject inferencePanel;

        public GameObject choicePanel;
        public Image choiceItemImg;
        public CustomTMPEffect choiceItemDescTMP;
        public TMP_Text[] choicesTMP;

        public GameObject inferenceDialog;
        public CustomTMPEffect inferenceDialogTMP;

        [HideInInspector]
        public bool isInChooseResult;
        [HideInInspector]
        public uint retryCount;
        [HideInInspector]
        public Item[] items;
        [HideInInspector]
        public int curItemIndex;
        [HideInInspector]
        public int findCount;

        private int lastChoice;
        public uint nextIndex;
        private GameManager gm;

        public void Initialize()
        {
            investigationDialogTMP = investigationDialog.transform.GetChild(0).GetComponent<CustomTMPEffect>();
            inferenceDialogTMP = inferenceDialog.transform.GetChild(0).GetComponent<CustomTMPEffect>();

            gm = GameManager.Instance;
        }

        public void SetInvestigationMode(uint nextIndex)
        {
            this.nextIndex = nextIndex;

            isInChooseResult = false;

            investigationUI.SetActive(true);
            inferencePanel.SetActive(false);
            investigationPanel.SetActive(true);
            
            items = new Item[gm.bgUI.transform.childCount];
            for (int i = 0; i < items.Length; ++i)
            {
                items[i] = gm.bgUI.transform.GetChild(i).GetComponent<Item>();
                items[i].imageComp.raycastTarget = true;
            }

            findCount = items.Length;

            gm.bgMtrl.SetFloat("_IsIn", 0.0f);
        }
        public void SetInferenceMode()
        {
            findAllItemFXAnimator.SetBool("IsFindAllItem", false);
            investigationPanel.SetActive(false);
            inferencePanel.SetActive(true);

            lastChoice = -1;
            curItemIndex = 0;
            retryCount = 2;

            SetChoicePanel();
        }
        public void SetChoicePanel()
        {
            isInChooseResult = false;

            inferenceDialog.SetActive(false);
            choicePanel.SetActive(true);

            choiceItemImg.sprite = items[curItemIndex].ItemImage;
            choiceItemDescTMP.SetText(items[curItemIndex].Desc);
            for (int i = 0; i < items[curItemIndex].ChoicesInfo.Length; ++i)
            {
                choicesTMP[i].transform.parent.gameObject.SetActive(true);
                choicesTMP[i].text = items[curItemIndex].ChoicesInfo[i].choiceStr;
            }
        }
        public void ChooseChoice(int choice)
        {
            isInChooseResult = true;

            choicePanel.SetActive(false);
            inferenceDialog.SetActive(true);

            for (int i = 0; i < items[curItemIndex].ChoicesInfo.Length; ++i)
                choicesTMP[i].transform.parent.gameObject.SetActive(false);

            string resultStr = items[curItemIndex].ChoicesInfo[choice].resultStr;
            if (lastChoice == choice)
                resultStr = "장난하는건가..";
            lastChoice = choice;

            inferenceDialogTMP.SetText(resultStr);
        }
        public void OnUpdate()
        {
            if (gm.IsKeyDownForDialogEvent())
            {
                if (isInChooseResult)
                {
                    if (inferenceDialogTMP.IsDoneTyping)
                    {
                        if (items[curItemIndex].CorrectIndex == lastChoice || --retryCount == 0)
                        {
                            if (items.Length == ++curItemIndex)
                            {
                                gm.scriptData.SetScript(nextIndex);
                                return;
                            }
                            lastChoice = -1;
                        }
                        SetChoicePanel();
                    }
                    else
                        inferenceDialogTMP.SkipTyping();
                }
                else if (!choiceItemDescTMP.IsDoneTyping)
                    choiceItemDescTMP.SkipTyping();
            }

            if (gm.IsKeyDown())
            {
                if (getItemAnimator.gameObject.activeInHierarchy)
                {
                    if (getItemAnimator.GetCurrentAnimatorStateInfo(0).IsName("Skip"))
                    {
                        getItemAnimator.gameObject.SetActive(false);
                        getItemAnimator.SetBool("Skip", false);
                    }
                    else
                        getItemAnimator.SetBool("Skip", true);
                }
            }
        }
        public void OnFindAllItems()
        {
            findAllItemFXAnimator.SetBool("IsFindAllItem", true);
        }
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