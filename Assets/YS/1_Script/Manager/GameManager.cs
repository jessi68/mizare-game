using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace YS
{
    public class GameManager : Singleton<GameManager>
    {
        enum SIDE_IMAGE
        {
            LEFT_SIDE,
            RIGHT_SIDE
        }

        #region Field
        public DialogStruct dialogStruct;
        public ChoiceStruct choiceStruct;
        public InvestigationStruct ivStruct;

        public GameObject bgUI;
        public Material bgMtrl;

        public TMP_Text logTMP;

        public ScriptData scriptData;
        public ItemData itemData;
        
        private Sprite[] charImgs = new Sprite[(int)CHARACTER_IMAGE_INDEX.MAX];

        // sideFX 코루틴 정보 (스킵 시 사용)
        private Coroutine[] sideFXCoroutine = new Coroutine[2];
        private Coroutine bgFXCoroutine;

        public InventoryComponent invenComp;

        private StringBuilder log = new StringBuilder();

        public delegate void OnUpdate();
        public event OnUpdate OnUpdateEvent;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            // 사용할 캐릭터들 이미지 로딩
            charImgs[(int)CHARACTER_IMAGE_INDEX.NONE] = null;
            charImgs[(int)CHARACTER_IMAGE_INDEX.MIZAR] = ResourceManager.GetResource<Sprite>("image001");
            charImgs[(int)CHARACTER_IMAGE_INDEX.ALCOR] = ResourceManager.GetResource<Sprite>("image018");
        }
        void Start()
        {
            dialogStruct.Initialize();
            choiceStruct.Initialize();
            ivStruct.Initialize();

            // 나중에 로드시 로드한 index값으로 설정
            scriptData.SetScript(0);
        }
        void Update()
        {
            OnUpdateEvent?.Invoke();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 게임 저장
        /// </summary>
        private void SaveGame()
        {

        }
        /// <summary>
        /// 게임 불러오기
        /// </summary>
        private void LoadGame()
        {

        }

        #region Dialog Event
        /// <summary>
        /// 다이얼로그 설정
        /// </summary>
        public void SetDialog(DialogEvent de)
        {
            ResetEffects();

            log.Append("<b>");
            log.Append(de.Name);
            log.Append("</b>\n<size=40>");
            log.Append(de.Script);
            log.Append("</size>\n");

            logTMP.SetText(log);
            dialogStruct.nameTMP.SetText(de.Name);
            dialogStruct.scriptTMP.SetText(de.Script);

            ScreenEffect(de.ScreenEffect);
            SetCharSetting(SIDE_IMAGE.LEFT_SIDE, de.LeftImage, de.LeftHighlight, de.LeftEffect);
            SetCharSetting(SIDE_IMAGE.RIGHT_SIDE, de.RightImage, de.RightHighlight, de.RightEffect);
        }
        /// <summary>
        /// 다이얼로그 이벤트 발생시 호출되는 함수
        /// </summary>
        public void OnDialogEvent(DialogEvent de)
        {
            // 마우스 클릭시 타이핑이 안끝났다면 타이핑 끝내고, 타이핑이 다 되어있는 상태라면 다음 다이얼로그 설정
            if (!dialogStruct.scriptTMP.IsDoneTyping)
            {
                ResetEffects();
                dialogStruct.scriptTMP.SkipTyping();
            }
            else
                scriptData.SetScript(de.NextIdx);
        }
        /// <summary>
        /// 다이얼로그 이벤트가 발생했는가
        /// </summary>
        /// <returns>발생했다면 true</returns>
        public bool IsKeyDownForDialogEvent()
        {
            bool result;

            // GameState이고
            result = InGameUIManager.IsGameState() &&
                     // 스페이스 키가 눌렸거나
                     Input.GetKeyDown(KeyCode.Space) ||
                     // UI가 아닌곳에 마우스 클릭 이벤트가 발생했을때
                     (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject());

            return result;
        }
        #endregion

        #region Choice Event
        /// <summary>
        /// 선택지 고르면 호출되는 이벤트 함수
        /// </summary>
        /// <param name="index">선택지 번호</param>
        public void OnChooseChoice(int index)
        {
            (scriptData.CurrentScript as ChoiceEvent).OnChooseChoice(index);
        }
        #endregion

        #region Investigation Event
        public void OnFindItem(Item item)
        {
            item.gameObject.SetActive(false);
            --ivStruct.findCount;
            invenComp.AddItem(item.index);
        }
        public void OnInference()
        {
            if (ivStruct.findCount == 0)
            {
                ivStruct.SetInferenceMode();
            }
            else
            {
                ivStruct.investigationDialog.SetActive(true);
                CancelInvoke(nameof(HideInferenceDialogTMP));
                ivStruct.investigationDialogTMP.SetText("아직 다 조사하지 않은 것 같군..");
                Invoke(nameof(HideInferenceDialogTMP), 3.0f);
            }
        }
        public void OnInferenceChoose(int choice)
        {
            ivStruct.ChooseChoice(choice);
        }
        private void HideInferenceDialogTMP()
        {
            ivStruct.investigationDialog.SetActive(false);
        }
        #endregion

        #region FX
        /// <summary>
        /// 화면 효과
        /// </summary>
        /// <param name="screenFX">적용할 효과</param>
        private void ScreenEffect(SCREEN_EFFECT screenFX)
        {
            switch (screenFX)
            {
                case SCREEN_EFFECT.FADE_IN:
                    bgFXCoroutine = StartCoroutine(FadeEffect(true, 1.0f));
                    break;
                case SCREEN_EFFECT.FADE_OUT:
                    bgFXCoroutine = StartCoroutine(FadeEffect(false, 1.0f));
                    break;
                case SCREEN_EFFECT.RED_FLASH:
                    bgMtrl.SetColor("_AddColor", Color.red);
                    Invoke("ResetFlash", 0.25f);
                    break;
            }
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
            dialogStruct.sideImg[(int)side].sprite = charImgs[(int)charImgIdx];
            dialogStruct.sideImg[(int)side].color = isHighlight ? Color.white : Color.gray;

            switch (charFX)
            {
                case CHARACTER_EFFECT_INDEX.SHAKE_HORIZONTAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_VERTICAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_RANDOM:
                    sideFXCoroutine[(int)side] = StartCoroutine(ShakeEffect(dialogStruct.sideImg[(int)side].gameObject.transform, 5, 0.5f, 0.01f, charFX));
                    break;

                case CHARACTER_EFFECT_INDEX.BOUNCE:
                    sideFXCoroutine[(int)side] = StartCoroutine(BounceEffect(dialogStruct.sideImg[(int)side].gameObject.transform, 3.0f));
                    break;
            }
        }
        private IEnumerator ShakeEffect(Transform target, float intensity, float time, float intervalTime, CHARACTER_EFFECT_INDEX type)
        {
            WaitForSeconds interval = CachedWaitForSeconds.Get(intervalTime);

            Vector3 curShakeVector = Vector3.zero;
            Vector3 dir = new Vector3(0.0f, 0.0f, 0.0f);
            float remainingTime = time;
            float curIntensity;

            switch (type)
            {
                case CHARACTER_EFFECT_INDEX.SHAKE_VERTICAL:
                    dir.x = 1.0f;
                    break;
                case CHARACTER_EFFECT_INDEX.SHAKE_HORIZONTAL:
                    dir.y = 1.0f;
                    break;
            }

            while (remainingTime > 0.0f)
            {
                if (type == CHARACTER_EFFECT_INDEX.SHAKE_RANDOM)
                    dir = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.right;
                else
                    dir = -dir;

                curIntensity = intensity * (remainingTime / time);

                target.position -= curShakeVector;
                curShakeVector = dir * curIntensity;
                target.position += curShakeVector;

                remainingTime -= intervalTime;
                yield return interval;
            }

            target.position -= curShakeVector;
        }
        private IEnumerator BounceEffect(Transform target, float time)
        {
            float t = 0.0f;
            WaitForSeconds wf = CachedWaitForSeconds.Get(0.01f);
            Bezier bezier = new Bezier();
            bezier.bezierPos = new Vector3[3]
            {
                target.position,
                target.position + Vector3.up * 100.0f,
                target.position
            };

            while (t <= 1.0f)
            {
                t += time * 0.01f;
                target.position = bezier.GetBezierPosition(t);
                yield return wf;
            }
        }
        private IEnumerator FadeEffect(bool isIn, float time)
        {
            WaitForSeconds wf = CachedWaitForSeconds.Get(0.01f);
            float curTime = 0.0f;

            bgMtrl.SetFloat("_IsIn", isIn ? 1.0f : 0.0f);
            
            while (curTime < time)
            {
                bgMtrl.SetFloat("_CurTime", curTime / time);
                yield return wf;
                curTime += 0.01f;
            }
        }
        private void ResetFlash()
        {
            bgMtrl.SetColor("_AddColor", Vector4.zero);
        }
        /// <summary>
        /// 효과 중간에 스킵될 수 있으므로, 효과들이 재생중이라면 종료시키고 이미지들의 위치 원상태로 복구하는 함수
        /// </summary>
        private void ResetEffects()
        {
            // 작동중인 효과 코루틴 멈추기
            if (sideFXCoroutine[0] != null) StopCoroutine(sideFXCoroutine[0]);
            if (sideFXCoroutine[1] != null) StopCoroutine(sideFXCoroutine[1]);
            if (bgFXCoroutine != null)      StopCoroutine(bgFXCoroutine);

            for (int i = 0; i < 2; ++i)
                dialogStruct.sideImg[i].transform.position = dialogStruct.SidePosition[i];

            bgMtrl.SetFloat("_CurTime", 1.0f);
            ResetFlash();
        }
        #endregion

        public void ChangeBackground(GameObject newBG)
        {
            Transform canvasTr = bgUI.transform.parent;
            RectTransform newBGTr = newBG.GetComponent<RectTransform>();

            newBGTr.SetParent(canvasTr, true);
            newBGTr.SetAsFirstSibling();
            newBGTr.anchoredPosition = Vector2.zero;
            newBGTr.anchorMin = Vector2.zero;
            newBGTr.anchorMax = Vector2.one;
            newBGTr.sizeDelta = bgUI.GetComponent<RectTransform>().sizeDelta;

            Destroy(bgUI);

            bgUI = newBG;
        }
        #endregion
    }
}